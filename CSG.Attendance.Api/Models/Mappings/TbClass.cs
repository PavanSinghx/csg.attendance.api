using System;
using System.Collections.Generic;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class TbClass
    {
        public TbClass()
        {
            TbClassList = new HashSet<TbClassList>();
            TbDailyClassListGrade = new HashSet<TbDailyClassListGrade>();
        }

        public int ClassId { get; set; }
        public string ClassDescription { get; set; }
        public int TeacherId { get; set; }

        public virtual TbTeacher Teacher { get; set; }
        public virtual ICollection<TbClassList> TbClassList { get; set; }
        public virtual ICollection<TbDailyClassListGrade> TbDailyClassListGrade { get; set; }
    }
}
