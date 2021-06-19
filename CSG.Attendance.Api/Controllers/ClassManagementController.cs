using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSG.Attendance.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSG.Attendance.Api.Controllers
{
    [Route("v1/class")]
    [ApiController]
    public class ClassManagementController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly ITeacherService teacherService;

        public ClassManagementController(IAuthenticationService authenticationService, ITeacherService teacherService)
        {
            this.authenticationService = authenticationService;
            this.teacherService = teacherService;
        }

        [HttpGet]
        [Route("firebaseid/{firebaseid}")]
        public async Task<object> GetClassSummary()
        {
            return await authenticationService.GetJwtToken(firebaseid);
        }

        [HttpGet]
        [Route("class/{classId}")]
        public async Task<object> GetClass(string firebaseid, int classId)
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
