using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Project.Utilities;

namespace Project.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public LoggingMiddleware(RequestDelegate requestDelegate, string path)
        {
            _next = requestDelegate;
            _path = path;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBody = context.Response.Body;

            try
            {
                using ( var memStream = new MemoryStream() )
                {
                    context.Response.Body = memStream;
                    var logObject = new LogObject
                    {
                        Request = new Request { Path = context.Request.Path, QueryString = context.Request.QueryString.ToString() }
                    };

                    await _next(context);

                    memStream.Position = 0;
                    var responseBody = JsonConvert.DeserializeObject(new StreamReader(memStream).ReadToEnd());

                    logObject.Response = new Response
                    {
                        StatusCode = context.Response.StatusCode,
                        Body = responseBody
                    };

                    File.AppendAllText(_path, JsonConvert.SerializeObject(logObject) + "\r\n");

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
