namespace EOM.TSHotelManagement.WebApi.Authorization
{
    using EOM.TSHotelManagement.Common;
    using EOM.TSHotelManagement.Contract;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Policy;
    using Microsoft.AspNetCore.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged || authorizeResult.Forbidden)
            {
                var response = new BaseResponse(BusinessStatusCode.Unauthorized,
                    LocalizationHelper.GetLocalizedString("PermissionDenied", "该账户缺少权限，请联系管理员添加"));

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json; charset=utf-8";

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DictionaryKeyPolicy = null
                });

                await context.Response.WriteAsync(json);
                return;
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}