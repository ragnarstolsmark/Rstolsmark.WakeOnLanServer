using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rstolsmark.Owin.PasswordAuthentication;

namespace Rstolsmark.WakeOnLanServer
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
            services
                .AddRazorPages()
                .AddSessionStateTempDataProvider();
            services.AddSession();
            services.Configure<PasswordAuthenticationOptions>(Configuration.GetSection("PasswordAuthenticationOptions"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<PasswordAuthenticationOptions> passwordAuthenticationsOptionsAccessor)
        {
            string basedir = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basedir, "data"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            if (!string.IsNullOrEmpty(passwordAuthenticationsOptionsAccessor.CurrentValue.HashedPassword))
            {
                app.UseOwin(pipeline =>
                {
                    pipeline.UsePasswordAuthentication(passwordAuthenticationsOptionsAccessor.CurrentValue);
                });
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
