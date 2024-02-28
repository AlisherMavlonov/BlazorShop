namespace RazorShop.Server.Services.Product;

public interface IProductService
{
    Task<ServiceResponse<List<Shared.Product>>> GetProductsAsync();
}