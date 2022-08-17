﻿using Microsoft.AspNetCore.Components.Forms;

namespace TangyWeb_Server.Service.IService
{
    public interface IFileUploadService
    {
        Task<string> UploadFile(IBrowserFile file);
        bool DeleteFile(string filePAth);
    }
}
