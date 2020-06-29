using API.Middleware.Swagger;
using AspNetCoreRateLimit;
using CoreLib.Interfaces;
using CoreLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        //private static readonly IHttpContextAccessor _ctx;

        /// <summary>
        /// Adds HTTP web request throttling services (a.k.a. rate limiting) via 'AspNetCoreRateLimit' library based on IP limiting.
        /// </summary>
        /// <remarks>ref: https://github.com/stefanprodan/AspNetCoreRateLimit/wiki</remarks>
        public static IServiceCollection ConfigureHttpRequestThrottlingByIp(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.Configure<IpRateLimitOptions>(config.GetSection("HttpRequestRateLimit"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            return services;
        }


        public static IServiceCollection AddCustomAPIVersioning(this IServiceCollection services)
        {

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();

                // opt.ApiVersionReader = ApiVersionReader.Combine(
                //  new HeaderApiVersionReader("X-Version"),
                //   new QueryStringApiVersionReader("ver", "version"));

                //opt.Conventions.Controller<TalksController>()
                //  .HasApiVersion(new ApiVersion(1, 0))
                //  .HasApiVersion(new ApiVersion(1, 1))
                //  .Action(c => c.Delete(default(string), default(int)))
                //    .MapToApiVersion(1, 1);

            });


            services.AddVersionedApiExplorer(
               options =>
               {
                   // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                   // note: the specified format code will format the version as "'v'major[.minor][-status]"
                   options.GroupNameFormat = "'v'VVV";

                   // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                   // can also be used to control the format of the API version in route templates
                   options.SubstituteApiVersionInUrl = true;
               });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath.getXmlCommentsFilePath());

                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type =  Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                    });

                   // options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } });


                });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IAppPermissions, AppPermissions>();
            services.AddScoped<IBankInterface, BankService>();
            services.AddScoped<IAccountInterface, AccountService>();
            services.AddScoped<IStatementInterface, StatementService>();

            return services;
        }

    }
}
