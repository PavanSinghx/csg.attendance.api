using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
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

        public Task<List<TbLearner>> GetAllRegisteredStudentsForClassAsync(int classId)
        {
            using (attendanceContext)
            {
                var learnerTask = this.attendanceContext.TbClassList.Where(l => l.ClassId == classId)
                                                    .Select(l => l.Learner)
                                                    .ToListAsync();

                return learnerTask;
            }
        }
    }
}
