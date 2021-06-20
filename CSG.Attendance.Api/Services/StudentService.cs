using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Repositories;
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
        private readonly IStudentRepository studentRepository;

        public StudentService(IOptionsMonitor<JwtSettings> monitor, IStudentRepository studentRepository)
        {
            this.jwtSettings = monitor.CurrentValue;
            this.studentRepository = studentRepository;
        }

        public Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync(int teacherId)
        {
            return this.studentRepository.GetAllRegisteredStudentsForTeacherAsync(teacherId);
        }
    }
}
