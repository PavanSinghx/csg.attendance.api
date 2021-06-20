using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllRegisteredStudentsForClassAsync(int classId);
        Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync(int teacherId);
    }
}