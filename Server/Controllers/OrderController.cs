using Microsoft.AspNetCore.Mvc;
using IOrderService = RazorShop.Server.Services.OrderService.IOrderService;

namespace RazorShop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
    {
        var result = await _orderService.GetOrders();

        return result;
    }
    
    [HttpGet("{orderId}")]
    public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrdersDetails(int orderId)
    {
        var result = await _orderService.GetOrderDetails(orderId);

        return result;

    }


}