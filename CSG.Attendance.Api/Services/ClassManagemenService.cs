using CSG.Attendance.Api.Extensions;
using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public class ClassManagemenService : IClassManagemenService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IRepository<TbTeacher> teacherRepository;
        private readonly IRepository<TbClassList> classListRepository;
        private readonly IRepository<TbLearner> learnerRepository;
        private readonly IRepository<TbClass> classRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMemoryCacheService memoryCacheService;

        public ClassManagemenService(IHttpContextAccessor httpContext, IRepository<TbTeacher> teacherRepository, IRepository<TbClassList> classListRepository, IRepository<TbLearner> learnerRepository,
                                     IRepository<TbClass> classRepository, IStudentRepository studentRepository, IMemoryCacheService memoryCacheService)
        {
            this.httpContext = httpContext;
            this.teacherRepository = teacherRepository;
            this.classListRepository = classListRepository;
            this.learnerRepository = learnerRepository;
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task ClearAttendance(int classId)
        {
            var firebaserUserId = this.httpContext.HttpContext.Items["firebaseid"].ToString();

            var teacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == firebaserUserId);

            var classEntries = await this.classListRepository.GetAllAsync(cl => cl.ClassId == classId && cl.Class.TeacherId == teacher.TeacherId);

            classEntries.ForEach(cl => cl.Attendance = false);

            await this.classListRepository.UpdateRangeAsync(classEntries);
        }

        public async Task<List<Student>> GetAllRegistered(int classId)
        {
            //add caching

            var firebaserUserId = this.httpContext.HttpContext.Items["firebaseid"].ToString();

            var teacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == firebaserUserId);

            var classEntries = await this.studentRepository.GetAllRegisteredStudentsForClassAsync(classId);

            var classList = classEntries.Select(cl => new Student
            {
                Firstnames = cl.Firstnames,
                Surname = cl.Surname,
                StudentId = cl.LearnerId
            }).ToList();

            return classList;
        }

        public async Task UpdateStudentsOnClassRegister(AddStudentRequest studentRequest)
        {
            //boundary checks

            var studentUpdateList = new List<TbClassList>();
            var studentAddList = new List<TbClassList>();

            var teacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == studentRequest.FirebaseUserId);

            foreach (var student in studentRequest.Students)
            {
                var studentEntry = await this.learnerRepository.FirstOrDefaultAsync(cl => cl.LearnerId == student.StudentId);

                var classListUpdate = new TbClassList
                {
                    ClassId = studentRequest.ClassId,
                    Active = student.IsActive
                };

                if (studentEntry == default)
                {
                    classListUpdate.Learner = new TbLearner
                    {
                        Firstnames = student.Firstnames,
                        Surname = student.Surname,
                        LearnerId = student.StudentId
                    };
                }
                else
                {
                    classListUpdate.LearnerId = studentEntry.LearnerId;
                }

                var classEntry = await this.classListRepository.FirstOrDefaultAsync(cl => cl.LearnerId == student.StudentId &&
                                                                                          cl.ClassId == studentRequest.ClassId);

                if (classEntry != default)
                {
                    classEntry.Active = student.IsActive;
                    studentUpdateList.Add(classEntry);

                    continue;
                }

                studentAddList.Add(classListUpdate);
            }

            if (studentAddList.Count > 0)
            {
                await this.classListRepository.AddRangeAsync(studentAddList);
            }

            if (studentUpdateList.Count > 0)
            {
                await this.classListRepository.UpdateRangeAsync(studentUpdateList);
            }
        }
    }
}
