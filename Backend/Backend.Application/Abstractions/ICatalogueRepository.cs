using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface ICatalogueRepository
{
    public Catalogue Create(Catalogue catalogue);
    //int GetLastId();

    public Catalogue? GetById(int id);

    public void Delete(Catalogue catalogue);

    public void AddGpa(Course course, Student student);
    public Catalogue UpdateCatalogue(Catalogue catalogue, int id);

    public double ComputeGpa(Student student,Course course);
}
