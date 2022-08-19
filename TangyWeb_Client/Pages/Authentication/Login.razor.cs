using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Web;
using Tangy_Models;
using TangyWeb_Client.Helper;
using TangyWeb_Client.Services;
using TangyWeb_Client.Services.IServices;


namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        private IAuthenticationService authenticationService { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        public SignInRequestDto SignInRequest = new();
        public bool IsProcessing { get; set; }
        public bool ShowLoginError { get; set; }


        public async Task LoginUser()
        {
            ShowLoginError = false;
            IsProcessing = true;
            var response = await authenticationService.Login(SignInRequest);
            if (response.IsAuthSuccessful)
            {
                var absUri = new Uri(navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absUri.Query);
                string returnUrl = queryParam["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    navigationManager.NavigateTo("/" + returnUrl);
                }
                else
                {
                    navigationManager.NavigateTo("/");
                }
                await jsRuntime.ToasterSucces("Login successful");

            }
            else
            {
                ShowLoginError = true;

            }
            IsProcessing = false;
        }
    }
}
