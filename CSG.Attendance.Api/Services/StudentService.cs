using CSG.Attendance.Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public class StudentService : IStudentService
    {
        private readonly JwtSettings jwtSettings;

        public StudentService(IOptionsMonitor<JwtSettings> monitor)
        {
            this.jwtSettings = monitor.CurrentValue;
        }
    }
}
