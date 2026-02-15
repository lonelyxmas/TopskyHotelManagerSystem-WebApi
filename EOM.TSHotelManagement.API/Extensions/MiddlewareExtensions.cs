using Microsoft.AspNetCore.Builder;

namespace EOM.TSHotelManagement.WebApi
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
