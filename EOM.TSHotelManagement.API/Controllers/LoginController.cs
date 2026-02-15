using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace EOM.TSHotelManagement.WebApi
{
    public class LoginController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly CsrfTokenConfig _csrfConfig;

        public LoginController(
            IAntiforgery antiforgery,
            IOptions<CsrfTokenConfig> csrfConfig)
        {
            _antiforgery = antiforgery;
            _csrfConfig = csrfConfig.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<CsrfTokenDto> GetCSRFToken()
        {
            var response = new SingleOutputDto<CsrfTokenDto>();

            try
            {
                var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
                var expiresAt = DateTime.Now.AddMinutes(_csrfConfig.TokenExpirationInMinutes);
                var needsRefresh = false;

                Response.Cookies.Append(_csrfConfig.CookieName, tokens.RequestToken);

                // 检查是否需要刷新
                var refreshThreshold = expiresAt.AddMinutes(-_csrfConfig.TokenRefreshThresholdInMinutes);
                if (DateTime.Now >= refreshThreshold)
                {
                    needsRefresh = true;
                }

                response.Data = new CsrfTokenDto
                {
                    Token = tokens.RequestToken ?? string.Empty,
                    ExpiresAt = expiresAt,
                    NeedsRefresh = needsRefresh
                };
            }
            catch (Exception ex)
            {
                response.Data = null;
            }

            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<CsrfTokenDto> RefreshCSRFToken()
        {
            return GetCSRFToken();
        }
    }
}
