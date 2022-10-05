﻿using System.Collections.ObjectModel;
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

    public async void PrepareOrder(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        var ordersList = await _orderRepository.GetOrderToPrepare();
        var orders = new List<Order>(ordersList);
        foreach (var order in orders.ToList())
        {
            ConsoleHelper.Print("I started a new order");
            var foodList = await _foodService.GetFoodFromOrder(order.FoodList);

            var sortedByComplexity = await _foodService.SortFoodByComplexity(foodList);
            Console.WriteLine($"Order with id: {order.Id} has a list of {foodList.Count} foods");

            //A simple order is one that does not imply big rank food, and can be done by cook nr 3 with rank 2 and proficiency 2
            var isSimpleOrder = await IsSimpleOrder(order);

            if (!isSimpleOrder)
            {
                ConsoleHelper.Print("I am a normal order");
                await _cookService.AddFoodToCookerList(order.Id, sortedByComplexity.ToList(), new List<Task>());
            }
            else
            {
                ConsoleHelper.Print("I am a special order");
                await _cookService.CallSpecialCooker(order.Id, sortedByComplexity.ToList(), new List<Task>());
            }

            await SendOrder(order);
            ConsoleHelper.Print($"Order with id {order.Id} was packed and sent in the kitchen", ConsoleColor.Magenta);
            orders.Remove(order);
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