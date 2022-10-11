using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Services.CookService;

public interface ICookService
{
    Task GenerateCooks();
    Task<ConcurrentBag<Cook>> GetAll();
    Task<Cook> GetById(int id);
    Task<Cook?> GetFreeCook();
    Task<Cook> GetCookerByRank(int rank);
    Task AddFoodToCookerList(Order order, IEnumerable<Food> foodList, Dictionary<int, List<Task>> tasks);
    Task CallSpecialCooker(Order order, IEnumerable<Food> foodList, Dictionary<int, List<Task>> tasks);
}