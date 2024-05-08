using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface ICatalogueRepository
{
    public Task<Catalogue> Create(Catalogue catalogue);
    //int GetLastId();
    public Task<List<Catalogue>> GetAll();

    public Task<Catalogue?> GetById(int id);

    public Task Delete(Catalogue catalogue);

    public void AddGpa(Course course, Student student);
    public Task<Catalogue> UpdateCatalogue(Catalogue catalogue, int id);

    public double ComputeGpa(Student student,Course course);
}
