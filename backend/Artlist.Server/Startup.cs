using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common;
using Artlist.Common.Interfaces;
using Artlist.Common.Models;
using Artlist.Server.Models;
using Artlist.Server.Models.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artlist.Server
{
    public class Startup
    {
        private string CORS_POLICY = "cors_policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            /* *** [App settings] *** */
            var appSettingsSection = Configuration.GetSection("AppSettings");
            //Now you can load this by IOptions<AppSettings>
            services.Configure<AppSettings>(appSettingsSection);
        
            //bind section to class
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();

            /* *** [Init DAL] *** */


            /* *** [Reg services] *** */
            IFileStore fileStore = new HDDFileStore(appSettings.Files.BaseFolder);
            services.AddSingleton<IFileStore>(fileStore);
            services.AddSingleton<IArtlistEngine>(new ArtlistEngineProxy(appSettings, fileStore));
            services.AddSingleton<AppSettings>(appSettings);

            
            /* *** [Else] *** */
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                                  builder =>
                                  {
                                      builder.WithOrigins(appSettings.CORS.FrontEndURL)
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials();
                                  });
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(CORS_POLICY);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<AppHub>("/apphub");
            });
        }
    }
}
