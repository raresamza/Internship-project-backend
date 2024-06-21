using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Schools.Update;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Update;

public record UpdateStudent(int studentId, StudentUpdateDto student) : IRequest<StudentDto>;
public class UpdateStudentHandler : IRequestHandler<UpdateStudent, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateStudentHandler> _logger;
    public UpdateStudentHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateStudentHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(UpdateStudent request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
            }
            var studentUpdateDto= new StudentUpdateDto { Address=student.Address,Age=student.Age,Name=student.Name,ParentEmail=student.ParentEmail,ParentName=student.ParentName,PhoneNumber=student.PhoneNumber };
            var newStudent = await _unitOfWork.StudentRepository.UpdateStudent(request.student, student.ID);
            _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");

            //return StudentDto.FromStudent(newStudent);
            return _mapper.Map<StudentDto>(newStudent);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in students at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
