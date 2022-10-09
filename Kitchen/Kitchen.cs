using Kitchen.Helpers;
using Kitchen.Services.CookingApparatusServices;
using Kitchen.Services.CookService;
using Kitchen.Services.FoodService;
using Kitchen.Services.OrderService;

namespace Kitchen.Kitchen;

public class Kitchen : IKitchen
{
    private readonly ICookService _cookService;
    private readonly IOrderService _orderService;
    private readonly IFoodService _foodService;
    private readonly ICookingApparatusServices _cookingApparatusServices;

    public Kitchen(ICookService cookService, ICookingApparatusServices cookingApparatusServices,
        IFoodService foodService, IOrderService orderService)
    {
        _cookService = cookService;
        _cookingApparatusServices = cookingApparatusServices;
        _foodService = foodService;
        _orderService = orderService;
    }

    public async Task InitializeKitchenParallelAsync()
    {
        var taskList = new List<Task>
        {
            Task.Run(() => _cookService.GenerateCooks()),
            Task.Run(() => _foodService.GenerateMenu()),
            Task.Run(() => _cookingApparatusServices.GenerateCookingApparatus())
        };

        await Task.WhenAll(taskList);
        ConsoleHelper.Print("Everything is ready", ConsoleColor.Green);
    }

    public async Task MaintainKitchen()
    {
        await _orderService.PrepareOrder();
    }
}