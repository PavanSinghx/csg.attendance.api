using CSG.Attendance.Api.Models.Request;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface ITeacherService
    {
        Task AddTeacherOnRegistration(RegisterTeacherRequest registerTeacher);
    }
}