public interface IProductTypeService
{
    Task<ServiceResponse<List<ProductType>>> GetProductTypes();
}