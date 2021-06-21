using CSG.Attendance.Api.Exceptions;
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
        private readonly IRepository<TbDailyClassListGrade> dailyClassRepository;
        private readonly string firebaseId;

        public StudentService(IHttpContextAccessor httpContext, IStudentRepository studentRepository, IRepository<TbLearner> learnerRepository,
                              IRepository<TbDailyClassListGrade> dailyClassRepository)
        {
            this.httpContext = httpContext;
            this.studentRepository = studentRepository;
            this.learnerRepository = learnerRepository;
            this.dailyClassRepository = dailyClassRepository;
            this.firebaseId = this.httpContext?.HttpContext?.Items["firebaseid"]?.ToString();
        }

        public Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync()
        {
            return this.studentRepository.GetAllRegisteredStudentsForTeacherAsync(this.firebaseId);
        }

        public async Task<List<DailyClassGrade>> GetDailyClassGradesAsync(int studentId, string startDate)
        {
            var hasParsed = DateTime.TryParse(startDate, out var parsedStartingDate);

            if (!hasParsed)
            {
                throw new InvalidDateTimeException(startDate);
            }

            var dayStart = new DateTime(parsedStartingDate.Year, parsedStartingDate.Month, parsedStartingDate.Day, 0, 0, 0, 0);

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
                        ClassName = classList.ClassDescription,
                        DayStart = startDate
                    });
                }
            }

            dailyGrades.ForEach(d => d.DayStart = startDate);

            dailyGrades.AddRange(classListEntries);

            return dailyGrades;
        }

        public async Task UpdateStudentGradeAttendanceAsync(List<DailyClassGrade> dailyClasses)
        {
            var dailyClassUpdateList = new List<TbDailyClassListGrade>();
            var dailyClassAddList = new List<TbDailyClassListGrade>();

            foreach (var dailyClass in dailyClasses)
            {
                if (dailyClass.ClassId == default || dailyClass.LearnerId == default)
                {
                    continue;
                }

                var hasParsed = DateTime.TryParse(dailyClass.DayStart, out var parsedStartingDate);

                if (!hasParsed)
                {
                    continue;
                }

                var dayStart = new DateTime(parsedStartingDate.Year, parsedStartingDate.Month, parsedStartingDate.Day, 0, 0, 0, 0);

                var dailyClassEntry = await this.dailyClassRepository.FirstOrDefaultAsync(cl => cl.LearnerId == dailyClass.LearnerId &&
                                                                                          cl.ClassId == dailyClass.ClassId &&
                                                                                          cl.DayStart == dayStart);

                if (dailyClassEntry == default)
                {
                    var entry = new TbDailyClassListGrade
                    {
                        ClassId = dailyClass.ClassId,
                        LearnerId = dailyClass.LearnerId,
                        DayStart = dayStart,
                        DailyAttendance = dailyClass.DailyAttendance,
                        Grade = dailyClass.Grade
                    };

                    dailyClassAddList.Add(entry);
                }
                else
                {
                    dailyClassEntry.DailyAttendance = dailyClass.DailyAttendance;
                    dailyClassEntry.Grade = dailyClass.Grade;
                    dailyClassUpdateList.Add(dailyClassEntry);
                }
            }

            if (dailyClassAddList.Count > 0)
            {
                await this.dailyClassRepository.AddRangeAsync(dailyClassAddList);
            }

            if (dailyClassUpdateList.Count > 0)
            {
                await this.dailyClassRepository.UpdateRangeAsync(dailyClassUpdateList);
            }
        }

    }
}
