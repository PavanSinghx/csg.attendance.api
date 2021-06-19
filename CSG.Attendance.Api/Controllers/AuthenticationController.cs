using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Models.Response;
using CSG.Attendance.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSG.Attendance.Api.Controllers
{
    [Route("v1/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly ITeacherService teacherService;

        public AuthenticationController(IAuthenticationService authenticationService, ITeacherService teacherService)
        {
            this.authenticationService = authenticationService;
            this.teacherService = teacherService;
        }

        [HttpGet]
        [Route("firebaseid/{firebaseid}")]
        public async Task<JwtResponse> GetJwtToken(string firebaseid)
        {
            return await authenticationService.GetJwtToken(firebaseid);
        }

        [HttpPost]
        public async Task RegisterTeacher(RegisterTeacherRequest teacherRequest)
        {
            await teacherService.AddTeacherOnRegistration(teacherRequest);
        }
    }
}
