﻿using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Actions;
using Backend.Application.Classrooms.Create;
using Backend.Application.Classrooms.Delete;
using Backend.Application.Classrooms.Queries;
using Backend.Application.Classrooms.Response;
using Backend.Application.Classrooms.Update;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Delete;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Application.Courses.Update;
using Backend.Application.Students.Queries;
using Backend.Domain.Models;
using Backend.Infrastructure.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassroomController : ControllerBase
{
    private readonly IMailService _service;
    private readonly IMediator _mediator;

    public ClassroomController(IMailService service, IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }

    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet]
    public async Task<ActionResult> GetAllClassrooms(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetClassrooms(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassroom(int id)
    {
        return Ok(await _mediator.Send(new GetClassroomById(id)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPost]
    public async Task<IActionResult> PostClassroom(ClassroomCreationDto classroom)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new CreateClassroom(classroom.Name, classroom.SchoolId)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClassroom(int id, ClassroomUpdateDto classroom)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateClassroom(id, classroom)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("add/student")]
    public async Task<IActionResult> AddStudentToClassroom(int studentId,int classroomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new AddStudentToClassroom(studentId,classroomId)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("remove/student")]
    public async Task<IActionResult> RemoveStudentFromClassroom(int studentId, int classroomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new RemoveStudentFromClassroom(studentId, classroomId)));
    }
    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("add/teacher")]
    public async Task<IActionResult> AddTeacherToClassroom(int teacherId, int classroomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new AddTeacherToClassroom(teacherId, classroomId)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("remove/teacher")]
    public async Task<IActionResult> RemoveTeacherFromClassroom(int teacherId, int classroomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new RemoveTeacherFromClassroom(teacherId, classroomId)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("assign/coruse")]
    public async Task<IActionResult> AssignCourseToClassroom(int coruseId, int classroomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new AssignCourseToClassroom(coruseId, classroomId)));
    }


    [Authorize(Roles = "Teacher,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassroom(int id)
    {
        return Ok(await _mediator.Send(new DeleteClassroom(id)));
    }
}
