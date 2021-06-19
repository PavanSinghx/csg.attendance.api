using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Request
{
    public class RegisterTeacherRequest
    {
        public string Firstnames { get; set; }
        public string Surname { get; set; }
        public string FirebaseUserId { get; set; }
    }
}
