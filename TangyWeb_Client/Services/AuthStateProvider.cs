using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Tangy_Common;
using TangyWeb_Client.Helper;

namespace TangyWeb_Client.Services
{
    public class AuthStateProvider: AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }
         

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(SD.LocalStorage.JwtToken);
            if (String.IsNullOrEmpty(token))
            {
                //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                //    new[]
                //    {
                //        new Claim(ClaimTypes.Name, "Pippo")

                //    }, SD.Auth.AuthenticationType)));
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(SD.Auth.AuthenticationScheme, token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), SD.Auth.AuthenticationType)));
         }
    }
}
