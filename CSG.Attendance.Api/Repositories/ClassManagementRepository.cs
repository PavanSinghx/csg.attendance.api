using CSG.Attendance.Api.Models.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public class ClassManagementRepository : Repository<TbTeacher>, IClassManagementRepository
    {
        private readonly AttendanceContext attendanceContext;

        public ClassManagementRepository(AttendanceContext attendanceContext)
        : base(attendanceContext)
        {
            this.attendanceContext = attendanceContext;
        }

        public async Task<bool> RemoveClassAndClassListAsync(int classId)
        {
            using (var transaction = await attendanceContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var classWithClassList = this.attendanceContext.TbClass.Include(c => c.TbClassList)
                                                                           .FirstOrDefault(c => c.ClassId == classId);

                    this.attendanceContext.RemoveRange(classWithClassList);

                    var saveChanges = await this.attendanceContext.SaveChangesAsync();

                    transaction.Commit();

                    return saveChanges > 0;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
