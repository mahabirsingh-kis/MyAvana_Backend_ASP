using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.DAL.Auth;
using MyAvana.Payments.Api.Contract;
using MyAvana.Payments.Api.Services;
using MyAvanaApi.Models.ViewModels;
using MyAvana.Framework.TokenService;
using NLog.Extensions.Logging;

namespace MyAvana.Payments
{
    public class Startup
    {
        private readonly string connection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connection = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
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
            //Add Lower case urls
            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddHttpClient();

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

            //TODO: Swap out with a real database in production
            services.AddDbContext<AvanaContext>(opt =>
            {
                // Configure the context to use an in-memory store.
                opt.UseSqlServer(connection, b => b.MigrationsAssembly("MyAvana.Payment.Api"));

            });
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.Configure<JWTSettings>(Configuration.GetSection("TokenAuthentication"));
            services.AddTransient<MyAvana.Logger.Contract.ILogger, Logger.Services.NLogServices>();
            services.AddTransient<IPaymentServices, PaymentServices>();
            services.AddTransient<IStripeServices, StripeServices>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IBillingService, BillingService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ClaimsPrincipal>(
               s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
