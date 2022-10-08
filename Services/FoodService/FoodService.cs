using Kitchen.Models;
using Kitchen.Repositories.FoodRepository;

namespace Kitchen.Services.FoodService;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public Task<IList<Food>> GetAll()
    {
        return _foodRepository.GetAll();
    }

    public Task<Food?> GetById(int id)
    {
        return _foodRepository.GetById(id);
    }

    public Task GenerateMenu()
    {
        return _foodRepository.GenerateMenu();
    }

    public Task<IList<Food>> GetFoodFromOrder(IEnumerable<int> foodIds)
    {
        return _foodRepository.GetFoodFromOrder(foodIds);
    }

    public Task<List<Food>> SortFoodByComplexity(IEnumerable<Food> foods)
    {
        return Task.FromResult(foods.OrderByDescending(food => food.Complexity).ToList());
    }

    public Task<Food?> GetFoodThatPreparesQuickest(IEnumerable<Food> foods)
    {
        return Task.FromResult(foods.MinBy(food => food.PreparationTime));
    }
}