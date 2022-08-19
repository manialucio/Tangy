using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Tangy_Models;
using TangyWeb_Client.Helper;
using TangyWeb_Client.Services;
using TangyWeb_Client.Services.IServices;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        private  IAuthenticationService authenticationService { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LogoutUser();
        }


        public bool IsProcessing { get; set; }
        public bool ShowLoginError { get; set; }


        public async Task LogoutUser()
        {
            IsProcessing = true;
            await authenticationService.Logout();
            navigationManager.NavigateTo("/login");
            IsProcessing = false;
        }
    }
}
