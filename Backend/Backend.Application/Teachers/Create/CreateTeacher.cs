using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Create;

public record CreateTeacher(int Age, int PhoneNumber, string Name, string Address, Subject Subject) : IRequest<TeacherDto>;

public class CreateTeacherHandler : IRequestHandler<CreateTeacher, TeacherDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTeacherHandler> _logger;
    public CreateTeacherHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateTeacherHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TeacherDto> Handle(CreateTeacher request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = new Teacher() { Address = request.Address, Subject = request.Subject, Age = request.Age, PhoneNumber = request.PhoneNumber, Name = request.Name };

            await _unitOfWork.BeginTransactionAsync();
            var createdTeacher = await _unitOfWork.TeacherRepository.Create(teacher);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
            //return TeacherDto.FromTeacher(createdTeacher);
            return _mapper.Map<TeacherDto>(teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in teacehr at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }


    }
}
