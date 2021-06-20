using CSG.Attendance.Api.Exceptions;
using CSG.Attendance.Api.Extensions;
using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Request;
using CSG.Attendance.Api.Models.Response;
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
        private readonly IClassManagementRepository classManagementRepository;

        private readonly string firebaseId;

        public ClassManagemenService(IHttpContextAccessor httpContext, IRepository<TbTeacher> teacherRepository, IRepository<TbClassList> classListRepository, IRepository<TbLearner> learnerRepository,
                                     IRepository<TbClass> classRepository, IStudentRepository studentRepository, IMemoryCacheService memoryCacheService, IClassManagementRepository classManagementRepository)
        {
            this.httpContext = httpContext;
            this.teacherRepository = teacherRepository;
            this.classListRepository = classListRepository;
            this.learnerRepository = learnerRepository;
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.memoryCacheService = memoryCacheService;
            this.classManagementRepository = classManagementRepository;
            this.firebaseId = this.httpContext?.HttpContext?.Items["firebaseid"]?.ToString();
        }

        public async Task DeleteClassAsync(int classId)
        {
            this.firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            if (!await this.EnsureClassBelongsToTeacherAsync(classId))
            {
                throw new ValidationException(classId.ToString(), "Failed to associate class (classId: {0}) with associated teacher.");
            }

            var cachedValue = this.memoryCacheService.RetrieveValue<string, TeacherCache>(this.firebaseId);

            var isNullOrEmpty = cachedValue?.Classes.IsNullOrEmpty() ?? true;

            if (!isNullOrEmpty)
            {
                cachedValue.Classes.Clear();
                this.memoryCacheService.SetValue<string, TeacherCache>(this.firebaseId, cachedValue);
            }

            await this.classManagementRepository.RemoveClassAndClassListAsync(classId);
        }

        public async Task CreateClassAsync(CreateClassRequest classRequest)
        {
            this.firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            var teacher = await this.memoryCacheService.GetOrCreateAsync<string, TeacherCache>(this.firebaseId, async cacheEntry =>
            {
                var existingTeacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == this.firebaseId);

                var cache = new TeacherCache
                {
                    TeacherId = existingTeacher.TeacherId
                };

                return cache;
            });

            var classEntry = new TbClass
            {
                ClassDescription = classRequest.ClassDescription ?? "",
                TeacherId = teacher.TeacherId
            };

            await this.classRepository.AddAsync(classEntry);

            var teacherCache = this.memoryCacheService.RetrieveValue<string, TeacherCache>(this.firebaseId);
            teacherCache.Classes?.Clear();

            this.memoryCacheService.SetValue<string, TeacherCache>(this.firebaseId, teacherCache);
        }

        public async Task ClearAttendance(int classId)
        {
            this.firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            var firebaserUserId = this.httpContext.HttpContext.Items["firebaseid"].ToString();

            var teacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == firebaserUserId);

            var classEntries = await this.classListRepository.GetAllAsync(cl => cl.ClassId == classId && cl.Class.TeacherId == teacher.TeacherId);

            classEntries.ForEach(cl => cl.Attendance = false);

            await this.classListRepository.UpdateRangeAsync(classEntries);
        }

        public async Task<List<Student>> GetAllRegistered(int classId)
        {
            this.firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            if (!await this.EnsureClassBelongsToTeacherAsync(classId))
            {
                throw new ValidationException(classId.ToString(), "Failed to associate class (classId: {0}) with associated teacher.");
            }

            var classEntries = await this.studentRepository.GetAllRegisteredStudentsForClassAsync(classId);

            return classEntries;
        }

        public async Task<List<ClassResponse>> GetClassSummary()
        {
            this.firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            var cachedValue = this.memoryCacheService.RetrieveValue<string, TeacherCache>(this.firebaseId);

            var isNullOrEmpty = cachedValue?.Classes?.IsNullOrEmpty() ?? true;

            if (!isNullOrEmpty)
            {
                return cachedValue.Classes;
            }

            var classListEntries = await this.classRepository.GetAllAsync(t => t.Teacher.FirebaseUid == this.firebaseId);

            var classList = classListEntries.Select(cl => new ClassResponse
            {
                ClassId = cl.ClassId,
                ClassDescription = cl.ClassDescription
            }).ToList();

            var teacher = await this.teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == this.firebaseId);

            var cache = new TeacherCache
            {
                TeacherId = teacher.TeacherId,
                Classes = classList
            };

            this.memoryCacheService.SetValue<string, TeacherCache>(this.firebaseId, cache);

            return classList;
        }

        public async Task UpdateStudentsOnClassRegister(AddStudentRequest studentRequest)
        {
            await ValidateUpdateStudents(studentRequest);

            var studentUpdateList = new List<TbClassList>();
            var studentAddList = new List<TbClassList>();

            foreach (var student in studentRequest.Students)
            {
                var studentEntry = student.StudentId == default ? null : await this.learnerRepository.FirstOrDefaultAsync(cl => cl.LearnerId == student.StudentId);

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
                        Surname = student.Surname
                    };

                    studentAddList.Add(classListUpdate);

                    continue;
                }
                else
                {
                    classListUpdate.LearnerId = studentEntry.LearnerId;
                }

                var classEntry = await this.classListRepository.FirstOrDefaultAsync(cl => cl.LearnerId == student.StudentId &&
                                                                                          cl.ClassId == studentRequest.ClassId);

                if (classEntry == default)
                {
                    studentAddList.Add(classListUpdate);
                }
                else
                {
                    classEntry.Attendance = student.Attendance;
                    classEntry.Active = student.IsActive;
                    studentUpdateList.Add(classEntry);
                }
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

        private async Task ValidateUpdateStudents(AddStudentRequest studentRequest)
        {
            if (studentRequest.ClassId == default)
            {
                throw new ValidationException("ClassId");
            }

            if (studentRequest.Students.IsNullOrEmpty())
            {
                throw new ValidationException("Students");
            }

            foreach (var student in studentRequest.Students)
            {
                student.Firstnames = student.Firstnames?.Trim();
                student.Surname = student.Surname?.Trim();

                student.Firstnames.ThrowIfNullEmptyOrWhiteSpace("Firstnames");
                student.Surname.ThrowIfNullEmptyOrWhiteSpace("Surname");

                if (!student.IsActive && student.Attendance)
                {
                    student.Attendance = false;
                }
            }

            if (!await this.EnsureClassBelongsToTeacherAsync(studentRequest.ClassId))
            {
                throw new ValidationException(studentRequest.ClassId.ToString(), "Failed to associate class (classId: {0}) with associated teacher.");
            }
        }

        private Task<bool> EnsureClassBelongsToTeacherAsync(int classId)
        {
            return this.classRepository.ExistsAsync(cl => cl.Teacher.FirebaseUid == this.firebaseId && cl.ClassId == classId);
        }
    }
}
