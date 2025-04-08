using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Homeworks.Actions;
using Backend.Application.Homeworks.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Homeworks.Queries;


public record GetHomeworkById(int homeworkId) : IRequest<HomeworkDto>;



public class GetHomeworkByIdHandler : IRequestHandler<GetHomeworkById, HomeworkDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GradeStudentHomeworkHandler> _logger;


    public GetHomeworkByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GradeStudentHomeworkHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<HomeworkDto> Handle(GetHomeworkById request, CancellationToken cancellationToken)
    {
        var homework = await _unitOfWork.HomeworkRepository.GetById(request.homeworkId);

        try
        {
            if (homework == null)
            {
                throw new NullCourseException($"The homework with id: {request.homeworkId} was not found!");
            }

            //return CourseDto.FromCourse(course);
            _logger.LogInformation($"Action in homework at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<HomeworkDto>(homework);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in homework at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
