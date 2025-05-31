using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Backend.Infrastructure;

public class StudentReportService : IStudentReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> GenerateReportAsync(int studentId)
    {
        var student = await _unitOfWork.StudentRepository.GetById(studentId);

        if (student == null)
            throw new Exception("Student not found");

        var grades = student.Grades
            .SelectMany(g => g.GradeValues)
            .ToList();

        var average = grades.Any() ? grades.Average() : 0;

        var totalParticipationPoints = student.StudentCoruses.Sum(sc => sc.ParticipationPoints);


        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(14));

                page.Header()
                    .Text($"Weekly Report for {student.Name}")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(10)
                    .Column(col =>
                    {
                        col.Item().Text($"Student Name: {student.Name}");
                        col.Item().Text($"Parent Email: {student.ParentEmail}");
                        col.Item().Text($"Average Grade: {average:F2}");
                        col.Item().Text($"Participation Points: {totalParticipationPoints}");

                        col.Item().PaddingTop(10).Text("Participation by Course:");
                        foreach (var sc in student.StudentCoruses)
                        {
                            col.Item().Text($" - {sc.Course?.Name ?? "Course"}: {sc.ParticipationPoints}");
                        }

                        col.Item().PaddingTop(10).Text("Grades:");
                        foreach (var grade in grades)
                        {
                            col.Item().Text($" - {grade}");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("f")).SemiBold();
                    });
            });
        });

        return pdf.GeneratePdf();
    }
}