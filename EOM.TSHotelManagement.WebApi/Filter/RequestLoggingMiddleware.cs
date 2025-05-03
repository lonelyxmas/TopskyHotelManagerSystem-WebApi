using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _softwareVersion;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        IConfiguration config)
    {
        _next = next;
        _softwareVersion = config["SoftwareVersion"] ?? GetDefaultVersion();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var repository = context.RequestServices
        .GetRequiredService<GenericRepository<OperationLog>>();

        var startTime = Stopwatch.GetTimestamp();
        var log = new OperationLog
        {
            OperationId = Guid.NewGuid().ToString("N"),
            OperationTime = DateTime.Now,
            RequestPath = context.Request.Path,
            HttpMethod = context.Request.Method,
            LoginIpAddress = context.Connection.RemoteIpAddress?.ToString(),
            QueryString = context.Request.QueryString.Value,
            SoftwareVersion = _softwareVersion,
            OperationAccount = context.User.Identity?.Name ?? "Anonymous",
            LogContent = $"{context.Request.Method} {context.Request.Path}",
            DataInsUsr = context.User.Identity?.Name ?? "Anonymous",
            DataInsDate = DateTime.Now,
        };

        context.Request.EnableBuffering();

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        Exception exception = null;
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            log.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.StatusCode = log.StatusCode;
            await HandleExceptionAsync(context, ex);
        }
        finally
        {
            log.ElapsedTime = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            log.StatusCode = context.Response.StatusCode;

            // 读取响应体
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);

            // 异常处理
            if (exception != null)
            {
                log.ExceptionMessage = exception.Message;
                log.ExceptionStackTrace = exception.StackTrace;
                log.LogLevel = EOM.TSHotelManagement.Common.Core.LogLevel.Critical;
            }
            else
            {
                log.LogLevel = context.Response.StatusCode >= 400
                    ? EOM.TSHotelManagement.Common.Core.LogLevel.Warning
                    : EOM.TSHotelManagement.Common.Core.LogLevel.Normal;
            }

            log.LogLevelName = log.LogLevel.ToString();
            try
            {
                await repository.InsertAsync(log);
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices
                    .GetRequiredService<ILogger<RequestLoggingMiddleware>>();
                logger.LogError(ex, "日志记录失败");
            }
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var errorResponse = new { error = exception.Message };
        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static long GetElapsedMilliseconds(long start, long stop)
    {
        return (long)((stop - start) * 1000 / (double)Stopwatch.Frequency);
    }

    private string GetDefaultVersion()
    {
        return GetType().Assembly.GetName().Version.ToString(3);
    }
}