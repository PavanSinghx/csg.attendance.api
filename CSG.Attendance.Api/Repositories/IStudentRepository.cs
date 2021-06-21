using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllRegisteredStudentsForClassAsync(int classId);
        Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync(string firebaseId);
        Task<List<DailyClassGrade>> GetDailyClassGradeForStudentAsync(int studentId, DateTime startDate);
        Task<List<ClassResponse>> GetAllClassesForStudentAsync(int studentId);
        Task<List<TbDailyClassListGrade>> GetDailyPeriodReportAsync(int studentId, DateTime startDate, DateTime endDate);
    }
}