using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using ShopDAL;
using Newtonsoft.Json.Serialization;


namespace ShopWebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //option 1
           // services.AddControllers();
            // services.AddDbContext<EfDebilContext>();
            //services.AddControllers(option =>
            //{
            //    option.Filters.Add<LogActionFilter>();
            //});


            //option2
            services.AddDbContext<ShopContext>(

                options => { options.UseSqlServer(Configuration.GetConnectionString("Shop")); });

            if (Environment.IsDevelopment())
                services.AddControllers(configure => configure.Filters.Add<LogActionFilter>());
            else
                services.AddControllers();




            string a = Configuration["Author"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           // app.UseMiddleware<ExceptionHandler>(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
