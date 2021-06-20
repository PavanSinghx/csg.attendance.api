using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Controllers
{
    [Route("v1/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await this.studentService.GetAllRegisteredStudentsForTeacherAsync();
        }

        [HttpGet]
        [Route("studentid/{studentId}")]
        public async Task<List<DailyClassGrade>> GetStudentsAsync(int studentId)
        {
            return await this.studentService.GetDailyClassGradesAsync(studentId);
        }
    }
}
