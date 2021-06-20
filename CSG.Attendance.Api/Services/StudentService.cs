using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Response;
using CSG.Attendance.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IStudentRepository studentRepository;
        private readonly IRepository<TbLearner> learnerRepository;
        private readonly string firebaseId;

        public StudentService(IHttpContextAccessor httpContext, IStudentRepository studentRepository, IRepository<TbLearner> learnerRepository)
        {
            this.httpContext = httpContext;
            this.studentRepository = studentRepository;
            this.learnerRepository = learnerRepository;
            this.firebaseId = this.httpContext?.HttpContext?.Items["firebaseid"]?.ToString();
        }

        public Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync()
        {
            return this.studentRepository.GetAllRegisteredStudentsForTeacherAsync(this.firebaseId);
        }

        public async Task<List<DailyClassGrade>> GetDailyClassGradesAsync(int studentId)
        {
            var utcNow = DateTime.UtcNow;
            var dayStart = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, 0);

            var dailyGrades = await this.studentRepository.GetDailyClassGradeForStudentAsync(studentId, dayStart);
            var classLists = await this.studentRepository.GetAllClassesForStudentAsync(studentId);

            //memory time trade off
            var classDictionary = dailyGrades.ToDictionary(cl => cl.ClassId);

            var classListEntries = new List<DailyClassGrade>();

            foreach (var classList in classLists)
            {
                if (!classDictionary.ContainsKey(classList.ClassId))
                {
                    classListEntries.Add(new DailyClassGrade
                    {
                        ClassId = classList.ClassId,
                        ClassName = classList.ClassDescription
                    });
                }
            }

            dailyGrades.AddRange(classListEntries);

            return dailyGrades;
        }
    }
}
