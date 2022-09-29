using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyAvana.DAL.Auth;
using MyAvanaApi.Models.Entities;

namespace MyAvana.AI.Api
{
    public class Startup
    {
        private readonly string _tensorFlowModelFilePath;
        private readonly string _mlnetModelFilePath;
        private readonly string connection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _tensorFlowModelFilePath = GetAbsolutePath(Configuration["MLModel:TensorFlowModelFilePath"]);
            _mlnetModelFilePath = GetAbsolutePath(Configuration["MLModel:MLNETModelFilePath"]);

            connection = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            TensorFlowModelConfigurator.Initialize();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<UserEntity, UserRoleEntity>().AddEntityFrameworkStores<AvanaContext>().AddDefaultTokenProviders();
            services.AddDbContext<AvanaContext>(option => option.UseSqlServer(connection, b => b.MigrationsAssembly("MyAvana.AI.Api")));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
