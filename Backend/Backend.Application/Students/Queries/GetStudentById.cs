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

public record GetStudentById(int studentId) : IRequest<StudentDto>;



public class GetStudentByIdHandler : IRequestHandler<GetStudentById, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentByIdHandler> _logger;
    public GetStudentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(GetStudentById request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
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
