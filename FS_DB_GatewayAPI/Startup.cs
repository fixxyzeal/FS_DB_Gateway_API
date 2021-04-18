using AutoMapper;
using BO.AutoMapperProFile;
using BO.Models.Mongo;
using BO.ViewModels;
using DAL;
using FS_DB_GatewayAPI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FS_DB_GatewayAPI
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
            // Auto Mapper Configurations

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProFile)));

            //Add Mongo
            MongSettings mongoSetting = new MongSettings() { ConnectionString = Environment.GetEnvironmentVariable("MONGO_HOST"), DatabaseName = "ZeallyStudio" };

            services.AddSingleton<IDatabaseSettings>(mongoSetting);
            services.AddSingleton<IMongoUnitOfWork, MongoUnitOfWork>();
            //Add DI
            services.AddScoped<MessageResult>();
            services.AddScoped<ILogService, ServiceLB.LogService.LogService>();
            //Add Cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );
            });
            //Add Health Check
            services.AddHealthChecks();

            services.AddControllers(opt =>
            {
                // Add ExceptionFilter
                opt.Filters.Add(typeof(ExceptionFilter));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FS_DB_GatewayAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FS_DB_GatewayAPI v1"));

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc");
                endpoints.MapControllers();
            });
        }
    }
}