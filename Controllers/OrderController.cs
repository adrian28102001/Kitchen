using System.Collections.Concurrent;
using System.Collections.Specialized;
using Kitchen.Models;
using Kitchen.Services.OrderHistoryService;
using Kitchen.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Controllers;

[ApiController]
[Route("/kitchen")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderHistoryService _orderHistoryService;
    private readonly SemaphoreSlim _semaphore;
    
    public OrderController(IOrderService orderService, IOrderHistoryService orderHistoryService)
    {
        _orderService = orderService;
        _orderHistoryService = orderHistoryService;
        _semaphore = new SemaphoreSlim(1);
    }

    [HttpGet]
    public ConcurrentBag<OrderHistory> GetOrderHistory()
    {
        return _orderHistoryService.GetAll();
    }
    
    [HttpPost]
    public async Task GetOrderFromKitchen([FromBody] Order? order)
    {
        if (order == null) return;
        try
        {
            Console.WriteLine($"An order with {order.Id} came in the kitchen");
            await _orderService.InsertOrder(order);
        }
        catch (Exception e)
        {
            //ignore
        }
    }
    // [HttpPost]
    // public async Task<ContentResult> PostOrder([FromBody] Order? order)
    // {
    //     if (order == null) return Content("Order is null");
    //     
    //     try
    //     {
    //         await _orderService.SendOrder(order);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine($"Failed to send order {order.Id}");
    //     }
    //     
    //     // var response =  await client.PostAsync(url, data);
    //     // _logger.LogInformation("Order "+ order.Id+" sent to kitchen");
    //     return Content("Hi");
    // }
}