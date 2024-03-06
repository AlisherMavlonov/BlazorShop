﻿namespace RazorShop.Shared;

public class OrderOverviewResponse
{
    public int  Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotslPrice { get; set; }
    public string Product { get; set; }
    public string ProductImageUrl { get; set; }
}