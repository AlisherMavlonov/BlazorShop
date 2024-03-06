using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RazorShop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("checkout"), Authorize]
    public async Task<ActionResult<string>> CreateChackoutSession()
    {
        var session = await _paymentService.CreateCheckoutSession();

        return Ok(session.Url);
    }






}