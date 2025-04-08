//using AutoMapper;
//using Backend.Application.Abstractions;
//using Backend.Application.Courses.Response;
//using Backend.Application.Students.Responses;
//using MediatR;
//using Microsoft.Extensions.Logging;

//namespace Backend.Application.Homeworks.Actions;


//public record SubmitHomework(int HomeworkId, int StudentId) : IRequest<CourseDto>;


//public class SubmitHomeworkHandler : IRequestHandler<SubmitHomework, CourseDto>
//{

//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly ILogger<SubmitHomeworkHandler> _logger;



//    public SubmitHomeworkHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubmitHomeworkHandler> logger)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//        _logger = logger;
//    }

//    public async Task<CourseDto> Handle(SubmitHomework request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var studentHomework = await _unitOfWork.StudentRepository.GetByStudentAndHomework(request.StudentId, request.HomeworkId);

//            if (studentHomework == null)
//                throw new Exception($"StudentHomework not found for StudentId={request.StudentId}, HomeworkId={request.HomeworkId}");

//            await _unitOfWork.BeginTransactionAsync();
//            _unitOfWork.StudentRepository.SubmitHomework(studentHomework);
//            await _unitOfWork.CommitTransactionAsync();

//            var course = await _unitOfWork.CourseRepository.GetById(studentHomework.Homework.CourseId);
//            if (course == null)
//                throw new Exception("Course not found for the submitted homework.");

//            return _mapper.Map<CourseDto>(course);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error submitting homework.");
//            throw;
//        }
//    }
//}
