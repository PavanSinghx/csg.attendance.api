using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public interface IClassManagementRepository
    {
        Task<bool> RemoveClassAndClassListAsync(int classId);
    }
}