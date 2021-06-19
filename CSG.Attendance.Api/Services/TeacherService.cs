﻿using CSG.Attendance.Api.Extensions;
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
    public class TeacherService : ITeacherService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IRepository<TbTeacher> teacherRepository;
        private readonly IRepository<TbClassList> classListRepository;
        private readonly IRepository<TbLearner> learnerRepository;
        private readonly IRepository<TbClass> classRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMemoryCacheService memoryCacheService;

        private JwtSettings jwtSettings { get; set; }

        private ClassSettings classSettings { get; set; }

        public TeacherService(IHttpContextAccessor httpContext, IOptions<ClassSettings> classSettings, IOptions<JwtSettings> jwtSettings, IRepository<TbTeacher> teacherRepository, IRepository<TbClassList> classListRepository,
                              IRepository<TbLearner> learnerRepository, IRepository<TbClass> classRepository, IStudentRepository studentRepository, IMemoryCacheService memoryCacheService)
        {
            this.jwtSettings = jwtSettings.Value;
            this.classSettings = classSettings.Value;
            this.httpContext = httpContext;
            this.teacherRepository = teacherRepository;
            this.classListRepository = classListRepository;
            this.learnerRepository = learnerRepository;
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task AddTeacherOnRegistration(RegisterTeacherRequest registerTeacher)
        {
            registerTeacher.Firstnames = registerTeacher.Firstnames?.Trim();
            registerTeacher.Surname = registerTeacher.Surname?.Trim();
            registerTeacher.FirebaseUserId = registerTeacher.FirebaseUserId?.Trim();

            registerTeacher.Firstnames.ThrowIfNullEmptyOrWhiteSpace("Firstnames");
            registerTeacher.Surname.ThrowIfNullEmptyOrWhiteSpace("Surname");
            registerTeacher.FirebaseUserId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            var cachedValue = this.memoryCacheService.RetrieveValue<string, int>(registerTeacher.FirebaseUserId);

            if (cachedValue != default)
            {
                return;
            }

            var teacherExists = await this.teacherRepository.ExistsAsync(t => t.FirebaseUid == registerTeacher.FirebaseUserId);

            if (teacherExists)
            {
                return;
            }

            var teacher = new TbTeacher
            {
                FirebaseUid = registerTeacher.FirebaseUserId,
                Firstnames = registerTeacher.Firstnames,
                Surname = registerTeacher.Surname
            };

            var defaultClassCount = this.classSettings.DefaultClassAmount;

            var classEntryList = new List<TbClass>(defaultClassCount);

            for (int i = 0; i < defaultClassCount; i++)
            {
                var classEntry = new TbClass
                {
                    TeacherId = teacher.TeacherId,
                    ClassDescription = ""
                };

                classEntryList.Add(classEntry);
            }

            teacher.TbClass = classEntryList;

            await this.teacherRepository.AddAsync(teacher);

            this.memoryCacheService.SetValue<string, int>(registerTeacher.FirebaseUserId, teacher.TeacherId);
        }
    }
}
