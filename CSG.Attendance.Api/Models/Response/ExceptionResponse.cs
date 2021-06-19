using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Models.Response
{
    public class ExceptionResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
