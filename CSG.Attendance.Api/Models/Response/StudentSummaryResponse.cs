using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Response
{
    public class StudentSummaryResponse
    {
        public int AttendancePercentage { get; set; }

        public decimal Grade { get; set; }

        List<ClassResponse> Classes { get; set; }
    }
}
