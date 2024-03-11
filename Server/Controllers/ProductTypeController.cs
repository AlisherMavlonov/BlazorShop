using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RazorShop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProductTypeController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypeController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
    {
        var response = await _productTypeService.GetProductTypes();
        return Ok(response);
    }


}