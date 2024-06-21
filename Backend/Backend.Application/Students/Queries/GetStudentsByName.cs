using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Students.Queries
{
    public record GetStudentsByName(string StudentName, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<StudentDto>>;

    public class GetStudentsByNameHandler : IRequestHandler<GetStudentsByName, PaginatedResult<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetStudentsByNameHandler> _logger;

        public GetStudentsByNameHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentsByNameHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedResult<StudentDto>> Handle(GetStudentsByName request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Handling GetStudentsByName with StudentName: {request.StudentName}, PageNumber: {request.PageNumber}, PageSize: {request.PageSize}");

                var students = await _unitOfWork.StudentRepository.GetByNames(request.StudentName);
                if (students == null)
                {
                    throw new ApplicationException("No students found with the given name");
                }

                var totalCount = students.Count;

                var pagedStudents = students
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var studentDtos = _mapper.Map<List<StudentDto>>(pagedStudents);

                _logger.LogInformation($"Fetched students by name at: {DateTime.Now}");

                return new PaginatedResult<StudentDto>(
                    request.PageNumber,
                    request.PageSize,
                    totalCount,
                    studentDtos
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching students by name at: {DateTime.Now}", ex);
                throw;
            }
        }
    }
}
