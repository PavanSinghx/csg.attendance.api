using System;
using System.Collections.Generic;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class TbTeacher
    {
        public TbTeacher()
        {
            TbClass = new HashSet<TbClass>();
        }

        public int TeacherId { get; set; }
        public string FirebaseUid { get; set; }
        public string Firstnames { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<TbClass> TbClass { get; set; }
    }
}
