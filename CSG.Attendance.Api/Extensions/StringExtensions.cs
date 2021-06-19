using CSG.Attendance.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Extensions
{
    public static class StringExtensions
    {
        public static void ThrowIfNullEmptyOrWhiteSpace(this string value, string fieldName)
        {
            if (value == default || string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(fieldName);
            }
        }
    }
}
