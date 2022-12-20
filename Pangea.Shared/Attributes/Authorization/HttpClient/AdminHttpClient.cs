using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Pangea.Shared.Attributes.Authorization.Extensions;
using Pangea.Shared.Attributes.Authorization.Models;
using System.Net.Http.Headers;

namespace Pangea.Shared.Attributes.Authorization.HttpClient
{
    public class AdminHttpClientService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly HttpContext _httpContext;

        public AdminHttpClientService(System.Net.Http.HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext
                ?? throw new ArgumentNullException("HttpContext is null in AdminClientService");
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetTokenFromRequest());
        }

        public async Task<string> GetUsernameById(int userId, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.GetAsync($"/api/Users/user/{userId}/username");

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            }

            return string.Empty;
        }

        public async Task<UserClaimsPayload> GetUserClaims(int userId, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.GetAsync($"/api/Users/user/{userId}/claims", cancellationToken);

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Get<UserClaimsPayload>(cancellationToken)!;
            }

            return new UserClaimsPayload();
        }


        private string? GetTokenFromRequest()
        {
            string accessToken = _httpContext.Request.Headers[HeaderNames.Authorization]!;
            return accessToken?.Replace("Bearer ", "");
        }
    }
}
