﻿using RazorShop.Client.Services.ProductTypeService;

public class ProductTypeService : IProductTypeService
{
    private readonly HttpClient _http;

    public ProductTypeService(HttpClient http)
    {
        _http = http;
    }

    public event Action? OnChange;

    public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    
    public async Task GetProductTypes()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
        ProductTypes = result.Data;
    }
}