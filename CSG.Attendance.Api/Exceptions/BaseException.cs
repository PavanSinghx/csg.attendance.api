using System;

namespace CSG.Attendance.Api.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException() { }
        public BaseException(string message) : base(message) { }
    }
}
