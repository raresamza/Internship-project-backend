using Microsoft.ML;
using Microsoft.EntityFrameworkCore;
using Backend.Domain.Models;
using System.Linq;
using Backend.Infrastructure.Contexts;
using Microsoft.Extensions.Logging;
using Backend.Application.Courses.Actions;

public class GradePredictionService
{
    private readonly AppDbContext _dbContext;
    private readonly MLContext _mlContext;
    private readonly ILogger<GradePredictionService> _logger;

    private const string ModelPath = "MLModels/gradeModel.zip";

    public GradePredictionService(AppDbContext dbContext, ILogger<GradePredictionService> logger)
    {
        _dbContext = dbContext;
        _mlContext = new MLContext();
        _logger = logger;
    }

    public ITransformer? LoadModel(out DataViewSchema schema)
    {
        if (!File.Exists(ModelPath))
        {
            _logger.LogError("🚫 Model file not found at: " + ModelPath);
            schema = null!;
            return null!;
        }

        var model = _mlContext.Model.Load(ModelPath, out schema);
        _logger.LogInformation("✅ Model loaded from: " + ModelPath);
        _logger.LogInformation("📦 Model schema:");
        foreach (var col in schema)
        {
            _logger.LogInformation($" - {col.Name} ({col.Type})");
        }
        return model;
    }

    public void SaveModel(ITransformer model, DataViewSchema schema)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ModelPath)!);
        _mlContext.Model.Save(model, schema, ModelPath);
    }

    public ITransformer TrainAndSaveModel()
    {
        var trainingData = new List<GradePredictionModel>();

        for (int i = 0; i < 1000; i++)
        {
            float avgGrade = 3f + (float)(i % 8); // more range: 3 to 10
            float participation = (i % 20);       // 0 to 19
            float predicted = (avgGrade * 0.7f) + (participation * 0.3f); // simulate a weighted influence

            trainingData.Add(new GradePredictionModel
            {
                AverageGrade = avgGrade,
                ParticipationPoints = participation,
                PredictedGrade = predicted
            });
        }

        var data = _mlContext.Data.LoadFromEnumerable(trainingData);

        _logger.LogInformation("Pipeline output schema:");
        foreach (var col in data.Schema)
            _logger.LogInformation($"  - {col.Name} ({col.Type})");

        var pipeline = _mlContext.Transforms
            .Concatenate("Features", nameof(GradePredictionModel.AverageGrade), nameof(GradePredictionModel.ParticipationPoints))
            .Append(_mlContext.Regression.Trainers.FastTree(
                labelColumnName: "Label",
                featureColumnName: "Features"
            ));

        var model = pipeline.Fit(data);

        var predictions = model.Transform(data);
        var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label");
        _logger.LogInformation($"R²: {metrics.RSquared}, MAE: {metrics.MeanAbsoluteError}, RMSE: {metrics.RootMeanSquaredError}");

        SaveModel(model, data.Schema);
        _logger.LogInformation("✅ Model trained and saved.");

        // Test prediction using a constant sample input
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<GradePredictionModel, GradePredictionResult>(model);
        var testInput = new GradePredictionModel
        {
            AverageGrade = 8f,
            ParticipationPoints = 10f
        };
        var testResult = predictionEngine.Predict(testInput);

        _logger.LogInformation($"🧪 Prediction test (8, 10) → {testResult.PredictedGrade}");

        return model;
    }

    public float PredictGrade(ITransformer model, float avgGrade, float participationPoints)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<GradePredictionModel, GradePredictionResult>(model);

        var result = predictionEngine.Predict(new GradePredictionModel
        {
            AverageGrade = avgGrade,
            ParticipationPoints = participationPoints
        });

        _logger.LogInformation($"[PREDICTION AFTER LOAD] (Grade: {avgGrade}, Points: {participationPoints}) → Predicted: {result.PredictedGrade}");

        return result.PredictedGrade;
    }


    public async Task PredictAndSaveForAllAsync()
    {
        var model = LoadModel(out var schema);
        if (model == null)
            throw new InvalidOperationException("Model not trained yet.");

        var studentCourses = await _dbContext.StudentCourses
            .Include(sc => sc.Student)
                .ThenInclude(s => s.Grades)
            .AsNoTracking()
            .ToListAsync();

        var predictions = studentCourses
            .Select(sc =>
            {
                float averageGrade = (float)sc.Student.Grades
                    .Where(g => g.CourseId == sc.CourseId)
                    .SelectMany(g => g.GradeValues)
                    .DefaultIfEmpty()
                    .Average();

                var predictionEngine = _mlContext.Model.CreatePredictionEngine<GradePredictionModel, GradePredictionResult>(model);
                var prediction = predictionEngine.Predict(new GradePredictionModel
                {
                    AverageGrade = averageGrade,
                    ParticipationPoints = sc.ParticipationPoints
                });

                return new StudentGradePrediction
                {
                    StudentId = sc.StudentId,
                    CourseId = sc.CourseId,
                    PredictedGrade = prediction?.PredictedGrade ?? 0f,
                    PredictionDate = DateTime.UtcNow
                };
            })
            .ToList();

        _dbContext.GradePredictions.RemoveRange(_dbContext.GradePredictions);
        await _dbContext.GradePredictions.AddRangeAsync(predictions);
        await _dbContext.SaveChangesAsync();
    }
}
