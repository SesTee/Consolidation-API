using API.Extensions;
using API.Middleware;
using AspNetCoreRateLimit;
using DomainClassLib.Data.Contexts;
using LoggerClassLib.Filters;
using LoggerClassLib.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
//using SchedulerLib;
using System;

[assembly: ApiConventionType(typeof(DefaultApiConventions))] // automatically add standard endpoints attributes
namespace API
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
            services.AddControllers();

            services.AddResponseCompression(opts => opts.EnableForHttps = true);

            // rate limiting
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.ConfigureHttpRequestThrottlingByIp(Configuration);

            services.AddDbContext<AppPermsContext>();

            services.AddServices();

            services.AddCustomAPIVersioning();
            services.AddCustomSwagger();

            services.AddHealthChecks();

            // Add Authentication Scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) => { return notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow; },
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Keys:Jwt:Issuer"],
                        ValidAudience = Configuration["Keys:Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Keys:Jwt:Key"]))
                    };
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(new TrackPerformanceFilter()); // performance tracking logger middleware coming from LoggerClassLib

            })
            .AddJsonOptions(options => // for Swagger JSON indentation
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
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

            app.UseHttpsRedirection();

            // Expose the API for outer domain requests
            app.UseCors(opts =>
                opts.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET", "POST", "OPTIONS"));

            app.UseIpRateLimiting();
            app.UseResponseCompression();

            app.AddCustomSwagger(provider);
            app.UseHealthChecks("/health");
            //app.UseCustomScheduler();

            //app.UseHttpsRedirection();

            // automatically add other properties to Serilog
            app.Use(async (ctx, next) =>
            {
                using (Serilog.Context.LogContext.PushProperty("IPAddress", ctx.Connection.RemoteIpAddress)) //add client ip
                {
                    await next();
                }
            });

            app.UseApiExceptionHandler();  // from custom helper assembly for logging Errors on application level coming from LoggerClassLib


            app.UseRouting();
            app.UseAuthentication();

            app.UseCustomAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }


    }
}
