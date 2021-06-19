using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int ExpiryTimeInHours { get; set; }
        public string ClaimsIdentifier { get; set; }
    }
}
