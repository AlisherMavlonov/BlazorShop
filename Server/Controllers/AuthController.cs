using Microsoft.AspNetCore.Mvc;
using RazorShop.Client.Shared;

namespace RazorShop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegistor request)
    {
        var response = await _authService.Register(
            new User
            {
                Email = request.Email
            }, 
            request.Password);
        if (!response.Seccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
    {
        var response = await _authService.Login(request.Email, request.Password);
        if (!response.Seccess)
        {
            return BadRequest(response);
        }

        return Ok(response);

    }
}