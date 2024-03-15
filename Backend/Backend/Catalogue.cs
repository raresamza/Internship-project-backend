using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Catalogue
    {
        public Classroom Classroom {  get; set; }
        public Catalogue (Classroom classroom)
        {
            Classroom = classroom;
        }
    }
}
