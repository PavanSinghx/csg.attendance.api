﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Request
{
    public class AddStudentRequest
    {
        public int ClassId { get; set; }

        public List<Student> Students { get; set; }
    }
}
