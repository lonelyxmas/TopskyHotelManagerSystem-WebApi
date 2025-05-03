using Autofac;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using EOM.TSHotelManagement.Shared;
using EOM.TSHotelManagement.WebApi.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace EOM.TSHotelManagement.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置DataProtection服务
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "docker")
            {
                services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));
            }
            else
            {
                services.AddDataProtection();
            }

            services.AddSingleton<IJwtConfigFactory, JwtConfigFactory>();
            services.AddSingleton<IMailConfigFactory, MailConfigFactory>();
            services.AddSingleton<ILskyConfigFactory, LskyConfigFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };
            });

            services.AddAuthorization(options =>
            {
                // 定义一个名为“ApiAccess”的策略
                options.AddPolicy("ApiAccess", policy =>
                {
                    policy.AuthenticationSchemes.Add("Bearer");
                    policy.RequireAuthenticatedUser();
                });

                // 使用上面定义的“ApiAccess”策略作为默认策略
                options.DefaultPolicy = options.GetPolicy("ApiAccess");
            });


            services.AddControllers(options =>
            {
                options.Conventions.Add(new AuthorizeAllControllersConvention());
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(opt =>
            {
                //时间格式化响应
                //opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            #region 配置全局路由
            //在各个控制器添加前缀（没有特定的路由前面添加前缀）
            services.AddMvc(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("api/[controller]/[action]"));
            });
            #endregion

            #region 注册Swagger服务 
            services.AddSwaggerGen(s =>
            {
                #region 注册 Swagger
                s.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "EOM.TSHotelManagement.Web", //定义Swagger文档的名称
                    Version = "version-1.0.0", //定义Swagger文档的版本号
                    Description = "Api Document", //定义Swagger文档的描述
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://mit-license.org/")
                    }, //定义Swagger文档的开源授权协议
                    TermsOfService = new Uri("https://www.oscode.top/"), //定义Swagger文档的服务支持主页
                    Contact = new OpenApiContact
                    {
                        Name = "Easy-Open-Meta",
                        Email = "eom-official@oscode.top",
                        Url = new Uri("https://www.oscode.top/")
                    }
                });
                //获取同解决方案下各分层的xml注释，一般获取业务逻辑层、实体类和接口层即可
                s.IncludeXmlComments(AppContext.BaseDirectory + "EOM.TSHotelManagement.Application.xml");
                s.IncludeXmlComments(AppContext.BaseDirectory + "EOM.TSHotelManagement.Common.Core.xml");
                s.IncludeXmlComments(AppContext.BaseDirectory + "EOM.TSHotelManagement.WebApi.xml");
                #endregion

                #region 正式发布时用
                //s.IncludeXmlComments("EOM.TSHotelManagement.Application.xml");
                //s.IncludeXmlComments("EOM.TSHotelManagement.Core.xml");
                //s.IncludeXmlComments("EOM.TSHotelManagement.WebApi.xml");
                #endregion
            });
            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", policy =>
                {
                    policy.WithOrigins(new string[] {
                        "http://localhost:5173",
                        "http://localhost:5174",
                        "https://tshotel.oscode.top"
                    })
                         .AllowAnyMethod()
                         .AllowAnyHeader();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                new InitializeConfig(_configuration).InitializeDatabase(app);
            }
            catch (Exception ex)
            {
                Console.WriteLine(LocalizationHelper.GetLocalizedString($"Database initialization failed: {ex.Message}. " +
                    "Please manually initialize the database or fix the error and retry.",$"数据库初始化失败：{ex.Message}，请手动执行初始化或修复错误后重试。 "));
                throw new Exception(LocalizationHelper.GetLocalizedString($"Database initialization failed: {ex.Message}. " +
                    "Please manually initialize the database or fix the error and retry.", $"数据库初始化失败：{ex.Message}，请手动执行初始化或修复错误后重试。 "));
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("MyCorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region 使用Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "EOM.TSHotelManagement Api Document");
            });
            #endregion

        }

        /// <summary>
        /// AutoFac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器,实现批量依赖注入的容器
            try
            {
                builder.RegisterType<SqlSugarClientFactory>()
                       .As<ISqlSugarClientFactory>()
                       .SingleInstance();

                builder.Register(c => c.Resolve<ISqlSugarClientFactory>().CreateClient())
                               .As<ISqlSugarClient>()
                               .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(GenericRepository<>))
                       .AsSelf()
                       .InstancePerLifetimeScope();

                builder.RegisterType<RequestLoggingMiddleware>()
                       .InstancePerDependency();

                builder.RegisterType<JWTHelper>().AsSelf().InstancePerLifetimeScope();
                builder.RegisterType<MailHelper>().AsSelf().InstancePerLifetimeScope();
                builder.RegisterType<LskyHelper>().AsSelf().InstancePerLifetimeScope();

                //程序集批量反射注入
                var assemblyService = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "EOM.TSHotelManagement.Application.dll"));
                builder.RegisterAssemblyTypes(assemblyService)
                    .AsImplementedInterfaces()
                    .InstancePerDependency()
                    .PropertiesAutowired();

                //注入加解密组件
                var encryptionService = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "jvncorelib.dll"));
                builder.RegisterAssemblyTypes(encryptionService)
                    .PropertiesAutowired();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }


    }
}
