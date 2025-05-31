using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GradePredictionController : ControllerBase
{
    private readonly GradePredictionService _predictionService;
    private readonly IMapper _mapper;


    public GradePredictionController(GradePredictionService predictionService, IMapper mapper)
    {
        _predictionService = predictionService;
        _mapper = mapper;
    }

    [HttpPost("predict")]
    public IActionResult Predict([FromBody] PredictionInputDto input)
    {
        var model = _predictionService.LoadModel(out var schema);
        if (model == null)
            return BadRequest("Model not trained.");

        // Map to GradePredictionModel (same type used during training)
        var mappedInput = new GradePredictionModel
        {
            AverageGrade = input.AverageGrade,
            ParticipationPoints = input.ParticipationPoints
        };

        var predictedGrade = _predictionService.PredictGrade(model, mappedInput.AverageGrade, mappedInput.ParticipationPoints);

        return Ok(new { PredictedGrade = predictedGrade });
    }


    [HttpPost("train")]
    public IActionResult TrainModel()
    {
        var model = _predictionService.TrainAndSaveModel();
        return Ok("Model trained and saved successfully.");
    }
}

public class PredictionInputDto
{
    public float AverageGrade { get; set; }
    public float ParticipationPoints { get; set; }
}

