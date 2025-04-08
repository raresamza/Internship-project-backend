using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;

namespace Backend.Application.Abstractions;

public interface IHomeworkRepository
{
    public Task<Homework?> GetById(int id);

    public Task<List<Homework>> GetAllHomeworks();

    public void AssignHomeworkToCourse(Course course, string title, string description, DateTime deadline);


}
