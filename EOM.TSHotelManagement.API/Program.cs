using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EOM.TSHotelManagement.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 加载多个配置文件
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Application.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Database.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Services.json", optional: true, reloadOnChange: true);
            var configuration = builder.Configuration;

            // Autofac 容器配置
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(AutofacConfigExtensions.ConfigureAutofacContainer);

            // 服务配置
            builder.Services.ConfigureDataProtection(configuration);
            builder.Services.ConfigureQuartz(configuration);
            builder.Services.ConfigureAuthentication(configuration);
            builder.Services.RegisterSingletonServices(configuration);
            builder.Services.ConfigureControllers();
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureCors(configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureXForward();

            // 构建应用
            var app = builder.Build();

            // 应用配置
            app.ConfigureEnvironment();
            app.ConfigureMiddlewares();

            app.InitializeDatabase();

            app.SyncPermissionsFromAttributes();
            app.ConfigureEndpoints();
            app.ConfigureSwaggerUI();

            app.Run();
        }
    }
}