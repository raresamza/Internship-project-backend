using Microsoft.ML.Data;

public class GradePredictionModel
{
    [LoadColumn(0)]

    public float AverageGrade { get; set; }
    [LoadColumn(1)]

    public float ParticipationPoints { get; set; }
    [LoadColumn(2), ColumnName("Label")]

    public float PredictedGrade { get; set; }
}

public class GradePredictionResult
{
    [ColumnName("Score")]
    public float PredictedGrade { get; set; }
}