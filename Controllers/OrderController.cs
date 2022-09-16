using System.Text;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kitchen.Controllers;

[ApiController]
[Route("/kitchen")]
public class OrderController : Controller
{
    [HttpGet]
    public ContentResult GetOrders()
    {
        return Content("Hello");
    }
    
    // GET
    [HttpPost]
    public async Task<ContentResult> PostOrder([FromBody] Order? order)
    {
        try
        {
            //process order
            //send finshed order object back to dining hall`
            Thread.Sleep(10000);
            if (order != null)
            {
                order.Status = Status.ReadyToBeServed;
                Console.WriteLine(
                    $"Order with id {order.Id} from table {order.TableId} was brought by waiter with id {order.WaiterId}");
            }


            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string url = Settings.DiningHallUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);
            var result = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to send order {order.Id}");
        }

        // var response =  await client.PostAsync(url, data);
        // _logger.LogInformation("Order "+ order.Id+" sent to kitchen");
        return Content("Hi");
    }
}