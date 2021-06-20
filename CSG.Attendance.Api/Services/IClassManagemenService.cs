using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface IClassManagemenService
    {
        Task<List<ClassResponse>> GetClassSummary();
        Task<List<Student>> GetAllRegistered(int classId);
        Task DeleteClassAsync(int classId);
        Task CreateClassAsync(CreateClassRequest classRequest);
        Task UpdateStudentsOnClassRegister(AddStudentRequest studentRequest);
    }
}