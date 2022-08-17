using Microsoft.JSInterop;

namespace TangyWeb_Server.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToasterSucces(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("ShowToastr","success",  message);

        }
        public static async ValueTask ToasterFail(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("ShowToastr", "error", message);

        }

        public static async ValueTask SweetAlert2Success(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("SweetAlert2", "success", message);

        }
        public static async ValueTask SweetAlert2Failure(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("SweetAlert2", "error", message);

        }


    }
}
