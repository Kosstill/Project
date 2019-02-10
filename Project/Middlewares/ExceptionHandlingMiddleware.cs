using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch ( Exception exception )
            {
                await HandlingExceptionAsync(context, exception);
            }
        }

        private static Task HandlingExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var message = new
            {
                Path = context.Request.Path,
                Message = exception.Message
            };

            var result = JsonConvert.SerializeObject(message);
            return context.Response.WriteAsync(result);
        }
    }
}
