using System.Collections.Concurrent;
using Kitchen.Helpers;
using Kitchen.Models;
using Kitchen.Repositories.CookRepository;
using Kitchen.Services.FoodService;

namespace Kitchen.Services.CookService;

public class CookService : ICookService
{
    private readonly ICookRepository _cookRepository;
    private readonly IFoodService _foodService;

    public CookService(ICookRepository cookRepository, IFoodService foodService)
    {
        _cookRepository = cookRepository;
        _foodService = foodService;
    }

    public Task GenerateCooks()
    {
        return _cookRepository.GenerateCooks();
    }

    public Task<ConcurrentBag<Cook>> GetAll()
    {
        return _cookRepository.GetAll();
    }

    public Task<Cook> GetById(int id)
    {
        return _cookRepository.GetById(id);
    }

    public Task<Cook?> GetFreeCook()
    {
        return _cookRepository.GetFreeCook();
    }

    public Task<Cook> GetCookerByRank(int rank)
    {
        return _cookRepository.GetCookerByRank(rank);
    }

    public async Task AddFoodToCookerList(Order order, IEnumerable<Food> foodList, List<Task> tasks)
    {
        var foods = new List<Food>(foodList);
        foreach (var food in foods.ToList())
        {
            switch (food.Complexity)
            {
                case 3:
                {
                    await PrepareFoodForCooker(order, food, tasks, foods);
                    break;
                }

                case 2:
                {
                    await PrepareFoodForCooker(order, food, tasks, foods);
                    break;
                }
                case 1:
                {
                    await PrepareFoodForCooker(order, food, tasks, foods);
                    break;
                }
            }
        }

        if (foods.Any())
        {
            await SleepFunctionCall(foods);
            await AddFoodToCookerList(order, foods, tasks);
        }

        await Task.WhenAll(tasks);
    }

    //This cooker is done special for simple orders who can be fast done
    public async Task CallSpecialCooker(Order order, IEnumerable<Food> foodList, List<Task> tasks)
    {
        var foods = new List<Food>(foodList);
        var cooker = await _cookRepository.GetSpecialCooker(2, 2);
        if (cooker != null)
        {
            foreach (var food in foods.ToList())
            {
                if (cooker.CookingList.Count < cooker.Proficiency)
                {
                    await AdjustCookerCookingList(order, cooker, food, foods, tasks);
                }
                else
                {
                    break;
                }
            }
        }

        if (foods.Any())
        {
            await SleepFunctionCall(foods);
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("All the food from this order was cooked");
    }

    private async Task SleepFunctionCall(IEnumerable<Food> foods)
    {
        var foodThatPreparesQuickest = await _foodService.GetFoodThatPreparesQuickest(foods);
        if (foodThatPreparesQuickest != null)
        {
            await SleepGenerator.Delay(2);
        }
    }

    private async Task PrepareFoodForCooker(Order order, Food food, ICollection<Task> tasks, ICollection<Food> foods)
    {
        var cooker = await _cookRepository.GetCookerByRank(food.Complexity);

        if (cooker.CookingList.Count < cooker.Proficiency)
        {
            await AdjustCookerCookingList(order, cooker, food, foods, tasks);
        }
        else
        {
            //There is no one above the main chef, so there is no one who could help him
            if (food.Complexity != 3)
            {
                await CallCookForHelp(order, food, tasks, foods);
            }
        }
    }

    private async Task CallCookForHelp(Order order, Food food, ICollection<Task> tasks, ICollection<Food> foods)
    {
        var helperCooker = await _cookRepository.GetCookerByRank(food.Complexity + 1);
        if (helperCooker.CookingList.Count < helperCooker.Proficiency)
        {
            ConsoleHelper.Print($"A helping {helperCooker.Name} come to help", ConsoleColor.Yellow);
            await AdjustCookerCookingList(order, helperCooker, food, foods, tasks);
        }
    }

    private Task AdjustCookerCookingList(Order order, Cook cooker, Food food, ICollection<Food> foods,
        ICollection<Task> tasks)
    {
        cooker.CookingList.Add(food);
        ConsoleHelper.Print($"I am {cooker.Name} and I will cook {food.Name}", ConsoleColor.DarkBlue);
        tasks.Add(CookFood(order, cooker.Id));
        foods.Remove(food);
        return Task.CompletedTask;
    }

    private async Task CookFood(Order order, int cookerId)
    {
        var cooker = await _cookRepository.GetById(cookerId);
        
        while (cooker.CookingList.Any())
        {
            var food = cooker.CookingList.FirstOrDefault();
            if (food == null) continue;
            if (food.CookingApparatus != null)
            {
                if (food.CookingApparatus.IsBusy)
                {
                    // ConsoleHelper.Print($"I tried to cook {food.Name} in {food.CookingApparatus.Name} but it's busy");
                    await SleepGenerator.Delay(1);
                }
                else
                {
                    await CookFoodInCookingApparatus(food, order, cooker, food.CookingApparatus);
                }
            }
            else
            {
                await SleepGenerator.Delay(food.PreparationTime);
                order.UpdatedOnUtc = DateTime.Now;
                cooker.CookingList.Remove(food);
                ConsoleHelper.Print($"I am {cooker.Name} and cooked {food.Name}", ConsoleColor.Green);
            }
        }
    }

    private static async Task CookFoodInCookingApparatus(Food food, Order order, Cook cooker,
        CookingApparatus cookingApparatus)
    {
        cookingApparatus.IsBusy = true;
        await SleepGenerator.Delay(food.PreparationTime);
        cookingApparatus.IsBusy = false;
        order.UpdatedOnUtc = DateTime.Now;
        cooker.CookingList.Remove(food);
        ConsoleHelper.Print($"I am {cooker.Name} and cooked {food.Name}", ConsoleColor.Green);
    }
}


// _orderHistoryService.Insert(new OrderHistory
// {
//     Id = IdGenerator.GenerateId(),
//     CookerId = cookerId,
//     FoodId = food.Id,
//     OrderId = order.Id
// });