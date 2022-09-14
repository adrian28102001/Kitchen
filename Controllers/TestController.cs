using System.Net;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kitchen.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IList<Food> GetHello()
    {
        string json = new WebClient().DownloadString("https://localhost:7299/api/Test/api/getfood");

        var items = JsonConvert.DeserializeObject<List<Food>>(json);
        return items;
        
    }
}