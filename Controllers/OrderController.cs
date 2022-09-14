using System.Text;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kitchen.Controllers;

[ApiController]
[Route("/order")]
public class OrderController : Controller
{
    // GET
    [HttpPost]
    public async Task<ContentResult> PostOrder([FromBody] Order? order)
    {
        //process order
        //send finshed order object back to dining hall
        Thread.Sleep(10000);
        if (order != null)
        {
            order.Status = Status.ReadyToBeServed;
            Console.WriteLine($"Order with id {order.Id} was brought by waiter with id {order.WaiterId}");
        }
        var json = JsonConvert.SerializeObject(order);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        const string url = Settings.DiningHallUrl;
        using var client = new HttpClient();

        var response =  await client.PostAsync(url, data);
        // var response =  await client.PostAsync(url, data);
        
        // _logger.LogInformation("Order "+ order.Id+" sent to kitchen");
        var result = await response.Content.ReadAsStringAsync();
        return Content("Hi");
    }
}