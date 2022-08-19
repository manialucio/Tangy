using Tangy_Models;

namespace TangyWeb_Client.Services.IServices
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseDto> RegisterUser(SignUpRequestDto signUpRequest);

        Task<SignInResponseDto> Login(SignInRequestDto signInRequest);

        Task Logout();
    }
}
