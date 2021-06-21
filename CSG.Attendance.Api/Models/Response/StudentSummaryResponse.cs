using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Response
{
    public class StudentSummaryResponse
    {
        public string ClassName { get; set; }

        public double Grade { get; set; }

        public int DaysAttended { get; set; }

        public int DaysMissed { get; set; }
    }
}
