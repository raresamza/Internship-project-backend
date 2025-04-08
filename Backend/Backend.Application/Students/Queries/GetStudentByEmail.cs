using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Students.Queries;

public record GetStudentByEmail(string email) : IRequest<StudentDto>;



public class GetStudentByEmailHandler : IRequestHandler<GetStudentByEmail, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentByEmailHandler> _logger;
    public GetStudentByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentByEmailHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(GetStudentByEmail request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetByEmail(request.email);
            if (student == null)
            {
                throw new ApplicationException("Student not found");
            }

            //return StudentDto.FromStudent(student);
            _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");

            //return _mapper.Map<StudentDto>(student);
            return _mapper.Map<StudentDto>(student);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in students at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }


    }
}
