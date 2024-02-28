namespace RazorShop.Server.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _context;

    public CategoryService(DataContext context)
    {
        _context = context;
    }
    
    public async Task<ServiceResponse<List<Category>>> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();

        return new ServiceResponse<List<Category>>()
        {
            Data = categories
        };

    }

    public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
    {
        var response = new ServiceResponse<List<Category>>();
        var result = await _context.Categories.ToListAsync();

        response.Data = result;

        return response;
    }
}