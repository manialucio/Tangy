using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Tangy_Common;
using Tangy_Models;
using TangyWeb_Client.Services.IServices;

namespace TangyWeb_Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authStateProvider = authStateProvider;
        }

        public async Task<SignInResponseDto> Login(SignInRequestDto signInRequest)
        {
            string content = JsonConvert.SerializeObject(signInRequest);
            var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/account/signin", bodyContent);

            string stringResponse = await response.Content.ReadAsStringAsync();
            var signInResponse = JsonConvert.DeserializeObject<SignInResponseDto>(stringResponse);
            if (response.IsSuccessStatusCode)
            {
                await _localStorageService.SetItemAsync(SD.LocalStorage.JwtToken, signInResponse.Token);
                await _localStorageService.SetItemAsync(SD.LocalStorage.UserDetails, signInResponse.User);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(SD.Auth.AuthenticationScheme, signInResponse.Token);
                ((AuthStateProvider)_authStateProvider).NotifyUserHasLoggedIn();

                return new SignInResponseDto() { IsAuthSuccessful = true };
            }
            else
            {
                return signInResponse;
            }

        }

        public async  Task Logout()
        {
            await _localStorageService.RemoveItemAsync(SD.LocalStorage.JwtToken);
            await _localStorageService.RemoveItemAsync(SD.LocalStorage.UserDetails);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            ((AuthStateProvider)_authStateProvider).NotifyUserHasLoggedOut();

        }

        public async Task<SignUpResponseDto> RegisterUser(SignUpRequestDto signUpRequest)
        {
            string content = JsonConvert.SerializeObject(signUpRequest);
            var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/account/signup", bodyContent);

            string stringResponse = await response.Content.ReadAsStringAsync();
            var signUpResponse = JsonConvert.DeserializeObject<SignUpResponseDto>(stringResponse);
            return signUpResponse;
        }
    }
}
