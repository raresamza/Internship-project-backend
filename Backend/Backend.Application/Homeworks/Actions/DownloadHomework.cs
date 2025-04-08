using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Backend.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Homeworks.Actions;

public record DownloadHomeworkFile(int studentId, int homeworkId) : IRequest<FileStreamResult>;


public class DownloadHomework
{
    public class DownloadHomeworkFileHandler : IRequestHandler<DownloadHomeworkFile, FileStreamResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DownloadHomeworkFileHandler> _logger;
        private readonly IFileStorageRepository _fileStorage;


        public DownloadHomeworkFileHandler(IUnitOfWork unitOfWork, ILogger<DownloadHomeworkFileHandler> logger, IFileStorageRepository fileStorage)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileStorage = fileStorage;
        }

        public async Task<FileStreamResult> Handle(DownloadHomeworkFile request, CancellationToken cancellationToken)
        {
            var submission = await _unitOfWork.StudentRepository.GetByStudentAndHomework(request.studentId, request.homeworkId);
            if (submission == null || string.IsNullOrEmpty(submission.FileUrl))
            {
                throw new FileNotFoundException("Submission not found.");
            }

            var stream = await _fileStorage.DownloadFileAsync(submission.FileUrl);

            if (stream == null)
                throw new Exception("File not found in storage.");

            var uri = new Uri(submission.FileUrl);
            string fileName = Path.GetFileName(uri.LocalPath);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = fileName
            };
        }
    }
}
