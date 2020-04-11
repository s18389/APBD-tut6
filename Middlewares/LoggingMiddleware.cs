using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tutorial6.Services;

namespace tutorial6.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IDbService service)
        {
            if (context.Request != null)
            {
                string path = context.Request.Path; // /api/students
                string method = context.Request.Method; // GET, POST
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                string allData = method + "\n" + path + "\n" + bodyStr + "\n" + queryString + "\n";
                // save to log file / log to database
                service.SaveLogData(allData);
            }



            if (_next != null) await _next(context); //executes next middleware
        }
    }

}
