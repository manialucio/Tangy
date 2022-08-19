using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tangy_Models;
using TangyWeb_Client.Helper;
using TangyWeb_Client.Services.IServices;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Register
    {
        [Inject]
        private  IAuthenticationService authenticationService { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        public SignUpRequestDto SignUpRequest = new SignUpRequestDto();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; } = false;
        public IEnumerable<string> Errors { get; set; } = new string[] { };


        public async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;
            var response = await authenticationService.RegisterUser(SignUpRequest);
            if (response.IsRegistrationSuccessful)
            {
                await jsRuntime.ToasterSucces("Registrazione ok");
                navigationManager.NavigateTo("/login");
            }
            else
            {
                ShowRegistrationErrors = true;
                Errors = response.Errors;

            }
            IsProcessing = false;
        }
    }
}
