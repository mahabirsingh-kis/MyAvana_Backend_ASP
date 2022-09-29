using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.Auth.Api.Contract;
using MyAvana.Auth.Api.Services;
using MyAvana.DAL.Auth;
using MyAvanaApi.Contract;
using MyAvanaApi.IServices;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using MyAvanaApi.Services;
using NLog.Extensions.Logging;

namespace MyAvana.Auth.Api
{
    public class Startup
    {
        private readonly string connection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connection = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "TestKey";
            })
            .AddJwtBearer("TestKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.Configure<FormOptions>(Option =>
            {
                Option.MultipartBodyLengthLimit = 200000000;
            });

            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.Configure<JWTSettings>(Configuration.GetSection("TokenAuthentication"));

            // Add ASP.NET Core Identity
            services.AddIdentity<UserEntity, UserRoleEntity>().AddEntityFrameworkStores<AvanaContext>().AddDefaultTokenProviders();
            services.AddDbContext<AvanaContext>(option => option.UseSqlServer(connection, b => b.MigrationsAssembly("MyAvana.Auth.Api")));

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;


                // User settings
                options.User.RequireUniqueEmail = true;
            });


            //Add Lower case urls
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                            .AllowAnyHeader();
                    })
            );
            services.AddResponseCaching();

            services.AddHttpClient();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICryptoService, RSACrypto>(s => new RSACrypto(Configuration.GetSection("Audience:Secret").Value));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<Logger.Contract.ILogger, Logger.Services.NLogServices>();
            
          
            services.AddTransient<ClaimsPrincipal>(
                s => s.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            app.UseAuthentication();
            
            //Enable CORS policy "AllowCors"  
            app.UseCors("AllowCors");
            
            app.UseMvc();
        }
    }
}
