using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Kitchen.Helpers;
using Kitchen.Models;
using Kitchen.Repositories.OrderRepository;
using Kitchen.Services.CookService;
using Kitchen.Services.FoodService;
using Newtonsoft.Json;

namespace Kitchen.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICookService _cookService;
    private readonly IFoodService _foodService;
    private readonly SemaphoreSlim _semaphore;

    public OrderService(IOrderRepository orderRepository, ICookService cookService, IFoodService foodService)
    {
        _orderRepository = orderRepository;
        _cookService = cookService;
        _foodService = foodService;
        _semaphore = new SemaphoreSlim(1);
    }

    public async Task InsertOrder(Order order)
    {
        // _orderRepository.Orders.CollectionChanged += PrepareOrder;
        await _orderRepository.InsertOrder(order);
    }

    private Task RemoveOrder(Order order)
    {
        _orderRepository.Orders.Remove(order);
        return Task.CompletedTask;
    }

    public Task<ObservableCollection<Order>> GetAll()
    {
        return _orderRepository.GetAll();
    }

    public async Task PrepareOrder()
    {
        while (true)
        {
            var orders = await _orderRepository.GetOrderToPrepare();
            if (orders.Any())
            {
                var order = orders.FirstOrDefault();
                if (order != null)
                {
                    var isSimpleOrder = await IsSimpleOrder(order);
                    var foodList = await _foodService.GetFoodFromOrder(order.FoodList);
                    var foodsByComplexity = await _foodService.SortFoodByComplexity(foodList);
                    ConsoleHelper.Print($"I started order with id {order.Id}, foodSize: {foodList.Count}");
                    if (isSimpleOrder)
                    {
                        CallWaiters(order, foodsByComplexity, isSimpleOrder);
                    }
                    else
                    {
                        await CallWaiters(order, foodsByComplexity, isSimpleOrder);
                    }

                    SendOrder(order);
                    ConsoleHelper.Print($"Order with id {order.Id} was packed and sent in the kitchen",
                        ConsoleColor.Magenta);
                    await RemoveOrder(order);
                }
            }
            else
            {
                ConsoleHelper.Print("There are no orders");
                await SleepGenerator.Delay(3);
                await PrepareOrder();
            }
        }
    }

    private async Task CallWaiters(Order order, IEnumerable<Food> orders, bool isSimpleOrder)
    {
        //A simple order is one that does not imply big rank food, and can be done by cook nr 3 with rank 2 and proficiency 2
        if (!isSimpleOrder)
        {
            ConsoleHelper.Print("I am a normal order");
            await _cookService.AddFoodToCookerList(order.Id, orders, new List<Task>());
        }
        else
        {
            ConsoleHelper.Print("I am a special order");
            await _cookService.CallSpecialCooker(order.Id, orders, new List<Task>());
        }
    }

    private async Task<bool> IsSimpleOrder(Order order)
    {
        var result = true;
        if (order.FoodList.Count > 6)
        {
            return false;
        }

        var foods = await _foodService.GetFoodFromOrder(order.FoodList);
        foreach (var food in foods)
        {
            if (food.Complexity > 2)
            {
                result = false;
            }
        }

        return result;
    }

    public async Task SendOrder(Order order)
    {
        try
        {
            Console.WriteLine($"I have sent the order with id: {order.Id} to kitchen");
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string url = Settings.DiningHallUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);
            var result = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            //ignore
        }
    }
}