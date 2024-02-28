﻿public class ProductService : IProductService
{
    private readonly DataContext _context;

    public ProductService(DataContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
    {
        var response = new ServiceResponse<List<Product>>()
        {
            Data = await _context.Products.Include(p=>p.Variants).ToListAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
    {
        var response = new ServiceResponse<Product>();
        var result = await _context.Products
            .Include(p=>p.Variants)
            .ThenInclude(v=>v.ProductType)
            .FirstOrDefaultAsync(p=>p.Id == productId);
        if (result == null)
        {
            response.Seccess = false;
            response.Message = "Sorry, but this product does not exist.";
        }
        else
        {
            response.Data = result;
        }
        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
    {
        var result = await _context.Products
            .Where(p => p.Category.Url.ToLower()
            .Equals(categoryUrl.ToLower()))
            .Include(p=>p.Variants)
            .ToListAsync();
        return new ServiceResponse<List<Product>>()
        {
            Data = result 
        };
    }
}