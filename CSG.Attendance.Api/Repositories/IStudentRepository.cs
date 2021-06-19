using CSG.Attendance.Api.Models.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public interface IStudentRepository
    {
        Task<List<TbLearner>> GetAllRegisteredStudentsForClassAsync(int classId);
    }
}