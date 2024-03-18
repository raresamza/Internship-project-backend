using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Absence
    {

        public Absence(Course course) {
            Course=course;
        }
        public DateTime Date { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"),"dd/MM/yyyy", null);
        public Course Course { get; set; }

        public void motivateAbsence(Student student, DateTime date)
        {

        }

        public Absence getAbsenceByDate(DateTime date)
        {
            return null;
        }

        public void increaseCount()
        {

        }
    }
}
