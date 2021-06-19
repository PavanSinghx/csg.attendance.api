using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Exceptions
{
    public class ValidationException : BaseException
    {
        const string message = "Failed to validate field {0}";

        public ValidationException(string field, string details = message)
        : base(string.Format(details, field))
        {

        }
    }
}
