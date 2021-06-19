using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Response
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public String ExpiryDate { get; set; }
    }
}
