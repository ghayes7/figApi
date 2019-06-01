using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FigApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FigApi.Models;
using FigApi.Models.Helpers;

namespace FigApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TeamRosters"));

            //register our Roster helper
            services.AddTransient<IRosterHelper, RosterHelper>();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

//            app.UseHttpsRedirection();
            app.UseMvc();

            //seed data
            if (Settings.SeedDatabase)
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        SeedData.SeedFakeData(db);
                    }
                }
            }
        }
    }
}
