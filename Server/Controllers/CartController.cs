using Microsoft.AspNetCore.Mvc;

namespace RazorShop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("products")]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(
        List<CartItem> cartItems)
    {
        var result = await _cartService.GetCartProducts(cartItems);

        return Ok(result);
    }
    
}