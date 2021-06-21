using CSG.Attendance.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync();
        Task<List<DailyClassGrade>> GetDailyClassGradesAsync(int studentId, string startDate);
        Task UpdateStudentGradeAttendanceAsync(List<DailyClassGrade> dailyClasses);
    }
}