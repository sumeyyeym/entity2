using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfConfigurations.ORM.Entities.ManyToMany
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public virtual List<Student> Students { get; set; }
        public ICollection<Student> Students { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
