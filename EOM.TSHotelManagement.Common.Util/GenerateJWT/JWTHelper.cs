using EOM.TSHotelManagement.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EOM.TSHotelManagement.Common.Util
{
    public class JWTHelper
    {
        private readonly IJwtConfigFactory _jwtConfigFactory;

        public JWTHelper(IJwtConfigFactory jwtConfigFactory)
        {
            _jwtConfigFactory = jwtConfigFactory;
        }

        public Dictionary<string, string> ClaimTypeMappings { get; set; } = new()
        {
            { "serialnumber", ClaimTypes.SerialNumber },
            { "unique_name", ClaimTypes.Name }
        };

        private JwtConfig GetJwtConfig() => _jwtConfigFactory.GetJwtConfig();

        /// <summary>
        /// JWT 加密生成
        /// </summary>
        public string GenerateJWT(ClaimsIdentity claimsIdentity)
        {
            var jwtConfig = GetJwtConfig();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtConfig.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddMinutes(jwtConfig.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtConfig.Audience,
                Issuer = jwtConfig.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// JWT 解密验证（返回 Claims 主体）
        /// </summary>
        public ClaimsPrincipal ValidateAndDecryptToken(string token)
        {
            var jwtConfig = GetJwtConfig();
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                ValidateIssuerSigningKey = true,
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new ApplicationException("Token 已过期");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new ApplicationException("无效的签名");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Token 验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取唯一序列号
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityTokenException"></exception>
        public string GetSerialNumber(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            var claimsPrincipal = ValidateAndDecryptToken(token);

            var serialClaim = claimsPrincipal.FindFirst(ClaimTypes.SerialNumber)
                ?? claimsPrincipal.FindFirst("serialnumber");

            return serialClaim?.Value ?? throw new SecurityTokenException("无法找到序列号声明");
        }

        /// <summary>
        /// 获取所有声明信息
        /// </summary>
        public IEnumerable<Claim> GetClaims(string token)
        {
            var principal = ValidateAndDecryptToken(token);
            return principal.Claims;
        }

        /// <summary>
        /// 获取 Token 过期时间
        /// </summary>
        public DateTime GetExpirationTime(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo.ToLocalTime();
        }
    }
}