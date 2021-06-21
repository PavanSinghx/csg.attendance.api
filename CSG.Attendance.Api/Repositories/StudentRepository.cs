using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public class StudentRepository : Repository<TbLearner>, IStudentRepository
    {
        private readonly AttendanceContext attendanceContext;
        private readonly IRepository<TbLearner> learnerRepository;
        private readonly IRepository<TbClassList> classListRepository;

        public StudentRepository(AttendanceContext attendanceContext, IRepository<TbLearner> learnerRepository, IRepository<TbClassList> classListRepository)
        : base(attendanceContext)
        {
            this.attendanceContext = attendanceContext;
            this.learnerRepository = learnerRepository;
            this.classListRepository = classListRepository;
        }

        public async Task<List<Student>> GetAllRegisteredStudentsForClassAsync(int classId)
        {
            var learnerTask = await this.attendanceContext.TbClassList.Where(l => l.ClassId == classId)
                                                                .Select(l =>
                                                                new Student
                                                                {
                                                                    Firstnames = l.Learner.Firstnames,
                                                                    Surname = l.Learner.Surname,
                                                                    Attendance = l.Attendance,
                                                                    IsActive = l.Active,
                                                                    StudentId = l.Learner.LearnerId
                                                                })
                                                                .ToListAsync();

            return learnerTask;
        }

        public async Task<List<Student>> GetAllRegisteredStudentsForTeacherAsync(string firebaseId)
        {
            var learnerTask = await this.attendanceContext.TbClassList.Where(cl => cl.Class.Teacher.FirebaseUid == firebaseId)
                                                                        .Select(l =>
                                                                        new Student
                                                                        {
                                                                            Firstnames = l.Learner.Firstnames,
                                                                            Surname = l.Learner.Surname,
                                                                            Attendance = l.Attendance,
                                                                            IsActive = l.Active,
                                                                            StudentId = l.Learner.LearnerId
                                                                        })
                                                                        .ToListAsync();

            return learnerTask;
        }

        public async Task<List<DailyClassGrade>> GetDailyClassGradeForStudentAsync(int studentId, DateTime startDate)
        {
            var learnerTask = await this.attendanceContext.TbDailyClassListGrade.Where(cl => cl.LearnerId == studentId && cl.DayStart == startDate)
                                                                          .Select(l =>
                                                                          new DailyClassGrade
                                                                          {
                                                                              ClassId = l.ClassId,
                                                                              ClassName = l.Class.ClassDescription,
                                                                              DailyAttendance = l.DailyAttendance,
                                                                              Grade = l.Grade,
                                                                              LearnerId = l.LearnerId
                                                                          })
                                                                          .ToListAsync();

            return learnerTask;
        }

        public async Task<List<ClassResponse>> GetAllClassesForStudentAsync(int studentId)
        {
            var learnerTask = await this.attendanceContext.TbClassList.Where(cl => cl.LearnerId == studentId)
                                                                .Select(l =>
                                                                new ClassResponse
                                                                {
                                                                    ClassDescription = l.Class.ClassDescription,
                                                                    ClassId = l.ClassId
                                                                })
                                                                .ToListAsync();

            return learnerTask;
        }

        public async Task<List<TbDailyClassListGrade>> GetDailyPeriodReportAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            var learnerTask = await this.attendanceContext.TbDailyClassListGrade.Include(cl => cl.Class)
                                                                                .Where(cl => cl.LearnerId == studentId &&
                                                                                       cl.DayStart <= endDate &&
                                                                                       cl.DayStart >= startDate)
                                                                                .ToListAsync();

            return learnerTask;
        }
    }
}
