using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Students.Actions;


public record SubmitHomeworkFile(int StudentId, int HomeworkId, IFormFile File) : IRequest<Unit>;

public class SubmitHomeworkFileHandler : IRequestHandler<SubmitHomeworkFile, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageRepository _blobStorageService;
    private readonly ILogger<SubmitHomeworkFileHandler> _logger;

    public SubmitHomeworkFileHandler(IUnitOfWork unitOfWork, IFileStorageRepository blobStorageService, ILogger<SubmitHomeworkFileHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<Unit> Handle(SubmitHomeworkFile request, CancellationToken cancellationToken)
    {
        var studentHomework = await _unitOfWork.StudentRepository.GetByStudentAndHomework(request.StudentId, request.HomeworkId);
        if (studentHomework == null)
            throw new Exception("Homework entry not found.");

        var student = await _unitOfWork.StudentRepository.GetById(request.StudentId);
        if (student == null)
            throw new Exception("Student not found.");

        var homework = await _unitOfWork.HomeworkRepository.GetById(request.HomeworkId);
        if (homework == null)
            throw new Exception("Homework not found.");

        var studentCourse = student.StudentCoruses.FirstOrDefault(sc => sc.CourseId == homework.CourseId);

        var currentDate = DateTime.UtcNow;


        bool submittedOnTime = currentDate <= homework.Deadline;
        bool isPresent = !await _unitOfWork.AbsenceRepository.HasAbsence(request.StudentId, homework.CourseId, currentDate);

        if (studentCourse != null && submittedOnTime && isPresent)
        {
            studentCourse.ParticipationPoints += 1;
        }

        // Sanitize student name
        var sanitizedName = student.Name.Replace(" ", "_").Trim();


        // Format timestamp
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm");

        // Generate the desired filename
        var blobName = $"submission_{sanitizedName}_{timestamp}.pdf";

        // Upload to Azure
        var fileUrl = await _blobStorageService.UploadFileAsync(request.File, blobName);

        // Save file info
        await _unitOfWork.BeginTransactionAsync();
        _unitOfWork.StudentRepository.SubmitHomework(studentHomework);
        studentHomework.FileUrl = fileUrl;

        await _unitOfWork.SaveAsync();
        await _unitOfWork.CommitTransactionAsync();

        return Unit.Value;
    }

}