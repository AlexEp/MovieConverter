using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artlist.Common.Interfaces;
using Artlist.Common.Interfaces.Repository;
using Artlist.Common.Models;
using Artlist.Core.Models;
using Artlist.Core.Models.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Artlist.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services )
        {

            /* *** [App settings] *** */
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            AppSettings appSettings = appSettingsSection.Get<AppSettings>();

            /* *** [Logger settings] *** */
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole();
            });


            /* *** [Reg DAL] *** */
            var connectionstring = appSettings.Database.ConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<ArtlistContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            var uploadFileRepository = new UploadFileRepository(optionsBuilder.Options);
            services.AddScoped<IUploadFileRepository>(s => uploadFileRepository);
            var convertedFileRepository = new ConvertedFileRepository(optionsBuilder.Options);
            services.AddScoped<IConvertedFileRepository>(s => convertedFileRepository);
            var thumbnailFileRepository = new ThumbnailFileRepository(optionsBuilder.Options);
            services.AddScoped<IThumbnailFileRepository>(s => thumbnailFileRepository);


            /* *** [Reg Swagger] *** */
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core API", Version = "v1" });
            });


            /* *** [Reg services] *** */
            IFileStore fileStore = new HDDFileStore(appSettings.Files.BaseFolder);
            IFileConverter fileConverter = new FileConverter(fileStore, appSettings);
            services.AddSingleton<IFileStore>(fileStore);
         
            var logger = loggerFactory.CreateLogger<ArtlistEngine>();

            //another way to create logger
            //var logger2 = services.BuildServiceProvider().GetRequiredService<ILogger<ArtlistEngine>>();
         

            services.AddSingleton<IArtlistEngine>(new ArtlistEngine(appSettings.Files.BaseFolder, 
                                                                        uploadFileRepository, 
                                                                        fileStore, 
                                                                        fileConverter, logger,
                                                                        convertedFileRepository,
                                                                        thumbnailFileRepository));


            var taskEnginelogger = loggerFactory.CreateLogger<TaskEngine>();
            var taskEngine = new TaskEngine(taskEnginelogger, 10);
            services.AddSingleton<ITaskEngine>(taskEngine);
            taskEngine.Start();

            services.AddSingleton<AppSettings>(appSettings);
            services.AddScoped<IFileConverter>(s => fileConverter);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
            });

        app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
