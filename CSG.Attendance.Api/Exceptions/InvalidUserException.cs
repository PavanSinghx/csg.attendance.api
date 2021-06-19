using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Exceptions
{
    public class InvalidUserException : BaseException
    {
        const string message = "User with Firebase User Id = {0} does not exist";

        public InvalidUserException(string firebaseUid, string details = message)
        : base(string.Format(details, firebaseUid))
        {

        }
    }
}
