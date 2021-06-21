using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Exceptions
{
    public class InvalidDateTimeException : BaseException
    {
        const string message = "Time string {0} is invalid";

        public InvalidDateTimeException(string date, string details = message)
        : base(string.Format(details, date))
        {

        }
    }
}
