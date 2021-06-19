using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public bool IsActive { get; set; }

        public string Firstnames { get; set; }

        public string Surname { get; set; }
    }
}
