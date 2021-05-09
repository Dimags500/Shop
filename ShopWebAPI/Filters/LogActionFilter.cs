using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine($"LogActionFilter.OnActionExecuting:{context.ActionDescriptor.DisplayName}");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string x = context.HttpContext.Request.Path.ToString();

            Debug.WriteLine(x);
            PrintLogToFile(context);
        }


        public void PrintLogToFile(ActionExecutingContext context)
        {
            //string Log = string.Format("{0} {1} ", DateTime.Now,
            //      context.HttpContext.Request.Path.Value + Environment.NewLine);

            string Log = string.Format("{0} {1} ", DateTime.Now,
                 context.ActionDescriptor.DisplayName + Environment.NewLine);

            string path = @"Data/LogActionFilter.txt";

            System.IO.File.AppendAllText(path, Log);


        }
    }
}
