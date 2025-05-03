using EOM.TSHotelManagement.Shared;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Util
{
    public class LskyHelper
    {
        private readonly ILskyConfigFactory lskyConfigFactory;

        public LskyHelper(ILskyConfigFactory lskyConfigFactory)
        {
            this.lskyConfigFactory = lskyConfigFactory;
        }

        public async Task<string> GetImageStorageTokenAsync()
        {
            var lskConfig = lskyConfigFactory.GetLskyConfig();
            using var httpClient = new HttpClient();
            var tokenRequest = new
            {
                email = lskConfig.Email,
                password = lskConfig.Password
            };

            var response = await httpClient.PostAsJsonAsync($"{lskConfig.BaseAddress}{lskConfig.GetTokenApi}", tokenRequest);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result?.Status == true ? result.Data.Token : null;
        }

        public async Task<string> UploadImageAsync(
                Stream fileStream,
                string fileName,
                string contentType,
                string token,
                int? strategyId = null)
        {
            try
            {
                if (fileStream == null || fileStream.Length == 0)
                    throw new ArgumentException(LocalizationHelper.GetLocalizedString("File stream cannot be empty", "文件流不能为空"));

                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException(LocalizationHelper.GetLocalizedString("File name cannot be empty", "文件名不能为空"));

                if (string.IsNullOrWhiteSpace(contentType))
                    throw new ArgumentException(LocalizationHelper.GetLocalizedString("Content type cannot be empty", "内容类型不能为空"));

                var lskConfig = lskyConfigFactory.GetLskyConfig();
                if (string.IsNullOrEmpty(lskConfig?.BaseAddress))
                    throw new InvalidOperationException(LocalizationHelper.GetLocalizedString("The base URL for the Lsky service is not configured.", "兰空图床基础地址未配置"));

                using var httpClient = new HttpClient();
                using var content = new MultipartFormDataContent();

                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                content.Add(fileContent, "file", fileName);

                if (strategyId.HasValue)
                {
                    content.Add(new StringContent(strategyId.Value.ToString()), "strategy_id");
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(
                    $"{lskConfig.BaseAddress}{lskConfig.UploadApi}",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"上传失败: {response.StatusCode} - {errorContent}");
                }

                var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
                return result?.Data?.Links?.Url ?? throw new Exception("响应中未包含有效URL");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class TokenResponse
        {
            public bool Status { get; set; }
            public TokenData Data { get; set; }
        }

        public class TokenData
        {
            public string Token { get; set; }
        }

        public class UploadResponse
        {
            public UploadData Data { get; set; }
        }

        public class UploadData
        {
            public UploadLinks Links { get; set; }
        }

        public class UploadLinks
        {
            public string Url { get; set; }
        }

    }
}
