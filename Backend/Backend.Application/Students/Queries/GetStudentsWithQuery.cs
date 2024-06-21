using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries
{
    public record GetStudentsWithQuery(string Query, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<StudentDto>>;

    public class GetStudentsWithQueryHandler : IRequestHandler<GetStudentsWithQuery, PaginatedResult<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetStudentsWithQueryHandler> _logger;

        public GetStudentsWithQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentsWithQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedResult<StudentDto>> Handle(GetStudentsWithQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Handling GetStudentsWithQuery with Query: {request.Query}, PageNumber: {request.PageNumber}, PageSize: {request.PageSize}");

                // Ensure the query parameter is not null
                var query = request.Query ?? string.Empty;

                List<Student> students;
                int totalCount;

                if (string.IsNullOrEmpty(query))
                {
                    students = await _unitOfWork.StudentRepository.GetWithQuery(null, request.PageNumber, request.PageSize);
                    totalCount = await _unitOfWork.StudentRepository.GetTotalCount(null);
                }
                else
                {
                    students = await _unitOfWork.StudentRepository.GetWithQuery(query, request.PageNumber, request.PageSize);
                    totalCount = await _unitOfWork.StudentRepository.GetTotalCount(query);
                }

                _logger.LogInformation($"Fetched students with query at: {DateTime.Now}");

                var studentDtos = _mapper.Map<List<StudentDto>>(students);

                return new PaginatedResult<StudentDto>(
                    request.PageNumber,
                    request.PageSize,
                    totalCount,
                    studentDtos
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching students with query at: {DateTime.Now}", ex);
                throw;
            }
        }
    }
}
