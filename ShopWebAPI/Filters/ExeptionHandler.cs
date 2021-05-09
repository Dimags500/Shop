using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopWebAPI
{

    public class ExceptionHandler
    {
        private RequestDelegate _RequestDelegate;
        private readonly IWebHostEnvironment env;
        public ExceptionHandler(RequestDelegate requestDelegate, IWebHostEnvironment env)
        { _RequestDelegate = requestDelegate; this.env = env; }

        public async Task Invoke(HttpContext httpContext)
        {
            try { await _RequestDelegate(httpContext); }

            catch (Exception e)
            {
                httpContext.Response.StatusCode = 500; httpContext.Response.ContentType = "application/json";
                if (env.EnvironmentName == "Development") 
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new { message = e.Message }));
            }
        }
    }

    
}
