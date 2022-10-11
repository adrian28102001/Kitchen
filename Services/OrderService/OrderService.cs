using System.Collections.ObjectModel;
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
    private readonly Semaphore _normalOrderSemaphore;
    private readonly Semaphore _specialOrderSemaphore;
    private readonly Semaphore _sendOrderSemaphore;

    public OrderService(IOrderRepository orderRepository, ICookService cookService, IFoodService foodService)
    {
        _orderRepository = orderRepository;
        _cookService = cookService;
        _foodService = foodService;
        _normalOrderSemaphore = new Semaphore(2, 2);
        _specialOrderSemaphore = new Semaphore(3, 3);
        _sendOrderSemaphore = new Semaphore(1, 1);
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
            var orders = await _orderRepository.GetOrdersToPrepare();
            if (orders.Any())
            {
                var order = orders.FirstOrDefault();
                if (order != null)
                {
                    var foodList = await _foodService.GetFoodFromOrder(order.FoodList);
                    var foodsByComplexity = await _foodService.SortFoodByComplexity(foodList);

                    if (await IsSimpleOrder(order))
                    {
                        Task.Run(() => CallWaiters(order, foodsByComplexity, foodList.Count));
                    }
                    else
                    {
                        Task.Run(async () => await CallWaiters(order, foodsByComplexity, foodList.Count));
                    }

                    Task.WaitAny();
                    await RemoveOrder(order);
                }
            }
            else
            {
                // ConsoleHelper.Print("There are no orders");
                await SleepGenerator.Delay(1);
                await PrepareOrder();
            }
        }
    }

    private async Task CallWaiters(Order order, IEnumerable<Food> foods, int foodListSize)
    {
        await Task.Run(async () =>
        {
            var isSimpleOrder = await IsSimpleOrder(order);
            //A simple order is one that does not imply big rank food, and can be done by cook nr 3 with rank 2 and proficiency 2
            if (isSimpleOrder)
            {
                _specialOrderSemaphore.WaitOne();
                ConsoleHelper.Print($"I started special order with id {order.Id}, food list size: {foodListSize}");
                await _cookService.CallSpecialCooker(order, foods, new Dictionary<int, List<Task>>());
                order.UpdatedOnUtc = DateTime.Now;
                Console.WriteLine("I am released");
                _specialOrderSemaphore.Release();
               
            }
            else
            {
                _normalOrderSemaphore.WaitOne();
                ConsoleHelper.Print($"I started normal order with id {order.Id}, food list size: {foodListSize}");
                await _cookService.AddFoodToCookerList(order, foods, new Dictionary<int, List<Task>>());
                order.UpdatedOnUtc = DateTime.Now;
                Console.WriteLine("I am released");
                _normalOrderSemaphore.Release();
            }

            _sendOrderSemaphore.WaitOne();
            await SendOrder(order);
            _sendOrderSemaphore.Release();
            ConsoleHelper.Print($"Order with id {order.Id} was packed and sent in the kitchen",
                ConsoleColor.Magenta);
        });
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

    private static async Task SendOrder(Order order)
    {
        await Task.Run(async () =>
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
        });
    }
}