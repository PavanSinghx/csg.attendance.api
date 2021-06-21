using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models
{
    public class DailyClassGrade
    {
        public int LearnerId { get; set; }

        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public string DayStart { get; set; }

        public int Grade { get; set; }

        public bool DailyAttendance { get; set; }
    }
}
