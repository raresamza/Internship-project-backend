using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;

namespace Backend.Application.Students.Actions;
public record GenerateStudentReport(int StudentId) : IRequest<byte[]>;

public class GenerateStudentReportHandler : IRequestHandler<GenerateStudentReport, byte[]>
{
    private readonly IStudentReportService _studentReportService;

    public GenerateStudentReportHandler(IStudentReportService studentReportService)
    {
        _studentReportService = studentReportService;
    }

    public async Task<byte[]> Handle(GenerateStudentReport request, CancellationToken cancellationToken)
    {
        // Your logic to create a PDF report for a student
        return await _studentReportService.GenerateReportAsync(request.StudentId);
    }
}
