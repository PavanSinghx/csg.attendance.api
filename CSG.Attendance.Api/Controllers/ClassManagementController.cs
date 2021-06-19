using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSG.Attendance.Api.Attributes;
using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Models.Response;
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
        private readonly IClassManagemenService classManagemenService;

        public ClassManagementController(IAuthenticationService authenticationService, ITeacherService teacherService, IClassManagemenService classManagemenService)
        {
            this.authenticationService = authenticationService;
            this.classManagemenService = classManagemenService;
        }

        [Authorize]
        [HttpGet]
        [Route("summary")]
        public async Task<List<ClassResponse>> GetClassSummaryAsync()
        {
            return await this.classManagemenService.GetClassSummary();
        }

        [Authorize]
        [HttpDelete]
        [Route("{classId}")]
        public async Task DeleteClassAsync(int classId)
        {
            await classManagemenService.DeleteClassAsync(classId);
        }

        [Authorize]
        [HttpPut]
        [Route("{classId}")]
        public async Task<object> UpdateClassAsync(string firebaseid, int classId)
        {
            return await authenticationService.GetJwtToken(firebaseid);
        }

        [Authorize]
        [HttpPost]
        [Route("{classId}")]
        public async Task<object> CreateClassAsync(string firebaseid, int classId)
        {
            return await authenticationService.GetJwtToken(firebaseid);
        }

        [Authorize]
        [HttpPut]
        [Route("classes")]
        public async Task UpdateClassesAsync([FromBody] AddStudentRequest studentRequest)
        {
            await classManagemenService.UpdateStudentsOnClassRegister(studentRequest);
        }
    }
}
