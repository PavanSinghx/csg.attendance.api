using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Response;
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
        [Route("studentid/{studentId}/date/{dateTime}")]
        public async Task<List<DailyClassGrade>> GetStudentsAsync(int studentId, string dateTime)
        {
            return await this.studentService.GetDailyClassGradesAsync(studentId, dateTime);
        }

        [HttpGet]
        [Route("studentid/{studentId}/startdate/{startDate}/enddate/{endDate}/report")]
        public async Task<List<StudentSummaryResponse>> GetStudentReport(int studentId, string startDate, string endDate)
        {
            return await this.studentService.AggregateDailyClassReportAsync(studentId, startDate, endDate);
        }

        [HttpPut]
        [Route("grade")]
        public async Task AddStudentGradeAttendanceAsync([FromBody] List<DailyClassGrade> dailyClasses)
        {
            await this.studentService.UpdateStudentGradeAttendanceAsync(dailyClasses);
        }
    }
}
