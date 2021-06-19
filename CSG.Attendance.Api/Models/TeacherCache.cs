using CSG.Attendance.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models
{
    public class TeacherCache
    {
        public int TeacherId { get; set; }

        public List<int> StudentIds { get; set; }
        public List<ClassResponse> Classes { get; set; }
    }
}
