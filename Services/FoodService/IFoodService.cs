using Kitchen.Models;

namespace Kitchen.Services.FoodService;

public interface IFoodService
{
    Task<IList<Food>> GetAll();
    Task<Food?> GetById(int id);
    Task GenerateMenu();
    Task<IList<Food>> GetFoodFromOrder(IEnumerable<int> foodIds);
    Task<List<Food>> SortFoodByComplexity(IEnumerable<Food> foods);
    Task<Food?> GetFoodThatPreparesQuickest(IEnumerable<Food> foods);
}