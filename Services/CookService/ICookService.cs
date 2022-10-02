using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Services.CookService;

public interface ICookService
{
    Task GenerateCooks();
    Task<ConcurrentBag<Cook>> GetAll();
    Task<Cook?> GetById(int id);
    Task<Cook?> GetFreeCook();
    Task<Cook> GetCookerByRank(int rank);
    Task AddFoodToCookerList(IEnumerable<Food> foodList, List<Task> tasks);
    Task CallSpecialCooker(IEnumerable<Food> toList, List<Task> tasks);
}