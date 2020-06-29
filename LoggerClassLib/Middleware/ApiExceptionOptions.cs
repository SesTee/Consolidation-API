using Microsoft.AspNetCore.Http;
using System;

namespace LoggerClassLib.Middleware
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
    }
}
