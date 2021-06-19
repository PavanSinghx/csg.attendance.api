using CSG.Attendance.Api.Models.Response;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface IAuthenticationService
    {
        Task<JwtResponse> GetJwtToken(string firebaseUid);
    }
}