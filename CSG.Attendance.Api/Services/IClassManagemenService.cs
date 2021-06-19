using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface IClassManagemenService
    {
        Task<List<ClassResponse>> GetClassSummary();
        Task DeleteClassAsync(int classId);
        Task UpdateStudentsOnClassRegister(AddStudentRequest studentRequest);
    }
}