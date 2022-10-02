using System.Collections.Specialized;
using Kitchen.Models;
using Kitchen.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Controllers;

[ApiController]
[Route("/kitchen")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task GetOrderFromKitchen([FromBody] Order? order)
    {
        if (order == null) return;
    
        try
        {
            Console.WriteLine($"An order with {order.Id} came in the kitchen");
            await _orderService.InsertOrder(order);
            _orderService.PrepareOrder(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get order {order.Id}", e);
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