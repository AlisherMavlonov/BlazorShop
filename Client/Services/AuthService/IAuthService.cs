﻿using RazorShop.Client.Shared;

namespace RazorShop.Client.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(UserRegistor request);
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    Task<bool> IsUserAuthenticated();
}