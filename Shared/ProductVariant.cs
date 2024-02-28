using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace RazorShop.Shared;

public class ProductVariant
{
    [JsonIgnore]
    public Product Product { get; set; }
    public int  ProductId { get; set; }
    public ProductType ProductType { get; set; }
    public int  ProductTypeId { get; set; }

    [Column(TypeName=("decimal(18,2)"))]
    public decimal Price { get; set; }
    [Column(TypeName=("decimal(18,2)"))]
    public decimal OriginalPrice { get; set; }
}