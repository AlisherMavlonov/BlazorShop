namespace RazorShop.Shared;

public class OrderDetailsProductResponse
{
    public int ProductId { get; set; }
    public string Titel { get; set; }
    public string ProductType { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
}