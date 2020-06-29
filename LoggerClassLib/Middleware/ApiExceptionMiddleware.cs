using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LoggerClassLib.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiExceptionOptions _options;

        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _options);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ApiExceptionOptions opts)
        {

            if (exception is BadRequestException)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Log.Error(exception, "An exception was caught in the API request pipeline. {HttpContext}",  context);

                return context.Response.WriteAsync(exception.Message);
            }

            var Id = Guid.NewGuid().ToString();

            var isAccessDenied = exception is UnauthorizedAccessException;            
            var status = isAccessDenied ? (short)HttpStatusCode.Unauthorized : (short)HttpStatusCode.InternalServerError;

            var error = new ApiError
            {
                Id = Id,
                Status = status,
                Title = isAccessDenied ? exception.Message : "Some kind of error occurred in the API.  Please use the id and contact our support team if the problem persists."
            };

            opts.AddResponseDetails?.Invoke(context, exception, error);
            
            Log.Error(exception, "An exception was caught in the API request pipeline. {CorrelationId}{HttpContext}", Id, context);

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;
            return context.Response.WriteAsync(result);
        }
    }
}
