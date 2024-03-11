﻿using System.Security.Claims;

namespace RazorShop.Server.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly DataContext _context;
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;

    public OrderService(DataContext context,
        ICartService cartService,
        IAuthService authService)
    {
        _context = context;
        _cartService = cartService;
        _authService = authService;
    }
    public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
    {
        var products = (await _cartService.GetDbCartProducts(userId)).Data;
        decimal totalPrice = 0;
        products.ForEach(product=> totalPrice += product.Price * product.Quantity);

        var orderItems = new List<OrderItem>();
        products.ForEach(product=> orderItems.Add(new OrderItem
        {
            ProductId = product.ProductId,
            ProductTypeId = product.ProductTypeId,
            Quantity = product.Quantity,
            TotalPrice = product.Price * product.Quantity
        }));

        var order = new Order()
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = totalPrice,
            OrderItems = orderItems
        };
        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(_context.CartItems
            .Where(ci=>ci.UserId == userId));
        await _context.SaveChangesAsync();

        return new ServiceResponse<bool>() { Data = true };
    }

    public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
    {
        var response = new ServiceResponse<List<OrderOverviewResponse>>();
        var orders = await _context.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .Where(z => z.UserId == _authService.GetUserId())
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var orderResponse = new List<OrderOverviewResponse>();
        orders.ForEach(z=> orderResponse.Add(new OrderOverviewResponse()
        {
            Id = z.Id,
            OrderDate = z.OrderDate,
            TotslPrice = z.TotalPrice,
            Product = z.OrderItems.Count > 1 ? 
                $"{z.OrderItems.First().Product.Title} and "+
                $"{z.OrderItems.Count - 1} more ... " :
                z.OrderItems.First().Product.Title,
            ProductImageUrl = z.OrderItems.First().Product.ImageUrl
            
        }));

        response.Data = orderResponse;

        return response;
    }

    public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
    {
        var response = new ServiceResponse<OrderDetailsResponse>();
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductType)
            .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            response.Success = false;
            response.Message = "Order not found.";
            return response;
        }

        var orderDetailsResponse = new OrderDetailsResponse()
        {
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Products = new List<OrderDetailsProductResponse>()
        };
        
        order.OrderItems.ForEach(item=>
            orderDetailsResponse.Products.Add(new OrderDetailsProductResponse()
            {
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Titel = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));
        response.Data = orderDetailsResponse;

        return response;


    }
}