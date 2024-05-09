using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Schools.Update;
using Backend.Application.Students.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetStudentByName(string studentName) : IRequest<StudentDto>;
public class GetStudentByNameHandler : IRequestHandler<GetStudentByName, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentByNameHandler> _logger;
    public GetStudentByNameHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentByNameHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(GetStudentByName request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetByName(request.studentName);
            if (student == null)
            {
                throw new ApplicationException("Student not found");
            }
            _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");

            //return StudentDto.FromStudent(student);
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
