using System;
using System.Collections.Generic;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class TbLearner
    {
        public TbLearner()
        {
            TbClassList = new HashSet<TbClassList>();
            TbDailyClassListGrade = new HashSet<TbDailyClassListGrade>();
        }

        public int LearnerId { get; set; }
        public string Firstnames { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<TbClassList> TbClassList { get; set; }
        public virtual ICollection<TbDailyClassListGrade> TbDailyClassListGrade { get; set; }
    }
}
