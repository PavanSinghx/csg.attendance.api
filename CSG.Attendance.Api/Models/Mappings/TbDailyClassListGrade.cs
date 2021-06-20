using System;
using System.Collections.Generic;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class TbDailyClassListGrade
    {
        public int ClassId { get; set; }
        public int LearnerId { get; set; }
        public DateTime DayStart { get; set; }
        public int Grade { get; set; }
        public bool DailyAttendance { get; set; }

        public virtual TbClass Class { get; set; }
        public virtual TbLearner Learner { get; set; }
    }
}
