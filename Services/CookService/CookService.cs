using System.Collections.Concurrent;
using Kitchen.Helpers;
using Kitchen.Models;
using Kitchen.Repositories.CookRepository;

namespace Kitchen.Services.CookService;

public class CookService : ICookService
{
    private readonly ICookRepository _cookRepository;

    public CookService(ICookRepository cookRepository)
    {
        _cookRepository = cookRepository;
    }

    public Task GenerateCooks()
    {
        return _cookRepository.GenerateCooks();
    }

    public Task<ConcurrentBag<Cook>> GetAll()
    {
        return _cookRepository.GetAll();
    }

    public Task<Cook?> GetById(int id)
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

    public async Task AddFoodToCookerList(IEnumerable<Food> foodList, List<Task> taskList)
    {
        var foods = new List<Food>(foodList);

        foreach (var food in foods.ToList())
        {
            switch (food.Complexity)
            {
                case 3:
                {
                    var cook = await _cookRepository.GetCookerByRank(food.Complexity);
                    if (cook.CookingList.Count <= cook.Proficiency)
                    {
                        cook.CookingList.Add(food);
                        taskList.Add(CookFood(cook.Id));
                        foods.Remove(food);
                    }

                    break;
                }
                case 2:
                {
                    var cook = await _cookRepository.GetCookerByRank(food.Complexity);
                    if (cook.CookingList.Count <= cook.Proficiency)
                    {
                        cook.CookingList.Add(food);
                        taskList.Add(CookFood(cook.Id));
                        foods.Remove(food);
                    }
                }
                    break;
                case 1:
                {
                    var cook = await _cookRepository.GetCookerByRank(food.Complexity);
                    if (cook.CookingList.Count <= cook.Proficiency)
                    {
                        cook.CookingList.Add(food);
                        taskList.Add(CookFood(cook.Id));
                        foods.Remove(food);
                    }
                }
                    break;
            }
        }

        if (foods.Any())
        {
            ConsoleHelper.Print("I got here, this means cooker I ask for is busy", ConsoleColor.Red);
            await SleepGenerator.Delay(foods.FirstOrDefault()!.PreparationTime);
            await AddFoodToCookerList(foods, taskList);
        }

        await Task.WhenAll(taskList);
        Console.WriteLine("All the food from this order was cooked");
    }

    private async Task CookFood(int cookerId)
    {
        var cooker = await _cookRepository.GetById(cookerId);
        var food = cooker?.CookingList.FirstOrDefault();

        if (food != null)
        {
            ConsoleHelper.Print($"I am {cooker?.Name} and I will cook {food.Name}");
            await SleepGenerator.Delay(food.PreparationTime);
            ConsoleHelper.Print($"I am {cooker?.Name} and cooked {food.Name}");
            cooker?.CookingList.Remove(food);
        }
    }
}