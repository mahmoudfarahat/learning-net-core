
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public IConfiguration config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services )
        {
             
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<DutchContext>();
            
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer( cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = config["Token:Issuer"],
                        ValidAudience = config["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]))
                };
                });
            services.AddDbContext<DutchContext>();

            services.AddTransient<DutchSeeder>();
            services.AddTransient<IMailService, NullMailService>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = 
            ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
            services.AddScoped<IDutchRepository, DutchRepository>();

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseDefaultFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();
            cfg.MapControllerRoute("Default",
                "/{Controller}/{action}/{id?}",
                  new { Controller = "App" , action = "Index" });
            });
              
            
        }
    }
}
