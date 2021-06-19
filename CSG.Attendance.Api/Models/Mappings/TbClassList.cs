using System;
using System.Collections.Generic;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class TbClassList
    {
        public int ClassId { get; set; }
        public int LearnerId { get; set; }
        public bool Attendance { get; set; }
        public bool Active { get; set; }

        public virtual TbClass Class { get; set; }
        public virtual TbLearner Learner { get; set; }
    }
}
