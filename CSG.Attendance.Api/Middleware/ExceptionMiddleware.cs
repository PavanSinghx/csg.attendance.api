using CSG.Attendance.Api.Exceptions;
using CSG.Attendance.Api.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception e)
            {
                await ResolveErrorAsync(e, context);
            }
        }

        private Task ResolveErrorAsync(Exception e, HttpContext context)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;

            switch (e)
            {
                case InvalidUserException _:
                case ValidationException _:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case UserNotFoundException _:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;
            }

            var exceptionResponse = new ExceptionResponse
            {
                Message = e.Message
            };

            var result = JsonConvert.SerializeObject(exceptionResponse);

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }
    }
}
