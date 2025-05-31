using Backend.Domain.Models;

public class StudentGradePrediction
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public float PredictedGrade { get; set; }

    public DateTime PredictionDate { get; set; }
}