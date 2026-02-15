using EOM.TSHotelManagement.Common;
using EOM.TSHotelManagement.Infrastructure;
using EOM.TSHotelManagement.WebApi.Authorization;
using EOM.TSHotelManagement.WebApi.Filters;
using jvncorelib.CodeLib;
using jvncorelib.EncryptorLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Quartz;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace EOM.TSHotelManagement.WebApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable(SystemConstant.Env.Code) == SystemConstant.Docker.Code)
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
                    .SetApplicationName("TSHotelManagementSystem");
            }
            else
            {
                services.AddDataProtection().SetApplicationName("TSHotelManagementSystem");
            }
        }

        public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                var jobs = configuration.GetSection(SystemConstant.JobKeys.Code).Get<string[]>() ?? Array.Empty<string>();

                foreach (var job in jobs)
                {
                    var reservationJobKey = $"{job}-Reservation";
                    var mailJobKey = $"{job}-Mail";
                    var imageHostingJobKey = $"{job}-ImageHosting";
                    var redisJobKey = $"{job}-Redis";

                    q.AddJob<ReservationExpirationCheckJob>(opts => opts
                        .WithIdentity(reservationJobKey)
                        .StoreDurably()
                        .WithDescription($"{reservationJobKey} 定时作业"));

                    q.AddTrigger(opts => opts
                        .ForJob(reservationJobKey)
                        .WithIdentity($"{reservationJobKey}-Trigger")
                        //.WithCronSchedule("* * * * * ? ")); // Debug Use Only 每秒执行一次
                        .WithCronSchedule("0 0 1 * * ?")); // 每天1:00 AM执行

                    q.AddJob<MailServiceCheckJob>(opts => opts
                        .WithIdentity(mailJobKey)
                        .StoreDurably()
                        .WithDescription($"{mailJobKey} 定时作业"));

                    q.AddTrigger(opts => opts
                        .ForJob(mailJobKey)
                        .WithIdentity($"{mailJobKey}-Trigger")
                        //.WithCronSchedule("* * * * * ? ")); // Debug Use Only 每秒执行一次
                        .WithCronSchedule("0 */5 * * * ?")); // 每5分钟执行一次

                    q.AddJob<ImageHostingServiceCheckJob>(opts => opts
                        .WithIdentity(imageHostingJobKey)
                        .StoreDurably()
                        .WithDescription($"{imageHostingJobKey} 定时作业"));

                    q.AddTrigger(opts => opts
                        .ForJob(imageHostingJobKey)
                        .WithIdentity($"{imageHostingJobKey}-Trigger")
                        //.WithCronSchedule("* * * * * ? ")); // Debug Use Only 每秒执行一次
                        .WithCronSchedule("0 */5 * * * ?")); // 每5分钟执行一次

                    q.AddJob<RedisServiceCheckJob>(opts => opts
                        .WithIdentity(redisJobKey)
                        .StoreDurably()
                        .WithDescription($"{redisJobKey} 定时作业"));

                    q.AddTrigger(opts => opts
                        .ForJob(redisJobKey)
                        .WithIdentity($"{redisJobKey}-Trigger")
                        //.WithCronSchedule("* * * * * ? ")); // Debug Use Only 每秒执行一次
                        .WithCronSchedule("0 */5 * * * ?")); // 每5分钟执行一次
                }
            });

            services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
                q.AwaitApplicationStarted = true;
                q.StartDelay = TimeSpan.FromSeconds(5);
            });
        }

        public static void ConfigureXForward(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                           ForwardedHeaders.XForwardedProto;

                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        public static void RegisterSingletonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ValidationFilter>();
            services.AddHttpClient("HeartBeatCheckClient", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            services.AddSingleton<RedisConfigFactory>();
            services.AddSingleton<JwtConfigFactory>();
            services.AddSingleton<MailConfigFactory>();
            services.AddSingleton<LskyConfigFactory>();
            services.AddSingleton<DataProtectionHelper>();
            services.Configure<CsrfTokenConfig>(configuration.GetSection("CsrfToken"));
            services.AddSingleton<EncryptLib>();
            services.AddSingleton<UniqueCode>();

            // RBAC: 注册基于权限码的动态策略提供者与处理器
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiAccess", policy =>
                {
                    policy.AuthenticationSchemes.Add("Bearer");
                    policy.RequireAuthenticatedUser();
                });
                options.DefaultPolicy = options.GetPolicy("ApiAccess")!;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "XSRF-TOKEN";
                options.HeaderName = "X-CSRF-TOKEN-HEADER";
                options.Cookie.HttpOnly = false;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SuppressXFrameOptionsHeader = false;
            });
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.Conventions.Add(new AuthorizeAllControllersConvention());
                options.RespectBrowserAcceptHeader = true;
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(new
                    {
                        Message = "验证失败",
                        Errors = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                    return result;
                };
            });

            // 全局路由配置
            services.AddMvc(opt =>
                opt.UseCentralRoutePrefix(new RouteAttribute("api/[controller]/[action]")));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddOpenApiDocument(config =>
            {
                config.Title = "TS酒店管理系统API说明文档";
                config.Version = "v1";
                config.DocumentName = "v1";

                config.OperationProcessors.Add(new CSRFTokenOperationProcessor());

                config.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Description = "Type into the textbox: your JWT token",
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .WithExposedHeaders("X-CSRF-TOKEN-HEADER")
#if DEBUG
                          .SetIsOriginAllowed(_ => true) // 开发环境下允许所有来源
#endif
                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
            });
        }
    }
}
