using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Exceptions
{
    public class UserNotFoundException : BaseException
    {
        const string message = "Unable to find user for Firebase User Id = {0}";

        public UserNotFoundException(string firebaseUid, string details = message)
        : base(string.Format(details, firebaseUid))
        {

        }
    }
}
