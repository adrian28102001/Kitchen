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


    public OrderService(IOrderRepository orderRepository, ICookService cookService, IFoodService foodService)
    {
        _orderRepository = orderRepository;
        _cookService = cookService;
        _foodService = foodService;
    }

    public Task InsertOrder(Order order)
    {
        _orderRepository.Orders.CollectionChanged += PrepareOrder;
        return _orderRepository.InsertOrder(order);
    }

    public Task<ObservableCollection<Order>> GetAll()
    {
        return _orderRepository.GetAll();
    }

    private Task<List<Order>> GetOldestOrders()
    {
        return _orderRepository.GetOldestOrders();
    }

    private Task<Order?> GetOrderToPrepare()
    {
        return _orderRepository.GetOrderToPrepare();
    }

    public async void PrepareOrder(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        var order = await GetOrderToPrepare();

        if (order == null) return;

        var foodList = await _foodService.GetFoodFromOrder(order.FoodList);
        var sortedByComplexity = await _foodService.SortFoodByComplexity(foodList);
        Console.WriteLine($"Order with id: {order.Id} has a list of {foodList.Count} foods");
        await _cookService.AddFoodToCookerList(sortedByComplexity.ToList(), new List<Task>());
    }


    public async Task SendOrder(Order order)
    {
        var json = JsonConvert.SerializeObject(order);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        const string url = Settings.DiningHallUrl;
        using var client = new HttpClient();

        var response = await client.PostAsync(url, data);
        var result = await response.Content.ReadAsStringAsync();
    }
}