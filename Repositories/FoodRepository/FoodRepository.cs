using Kitchen.Models;
using Kitchen.Services.CookingApparatusServices;

namespace Kitchen.Repositories.FoodRepository;

public class FoodRepository : IFoodRepository
{
    private readonly IList<Food> _foods;
    private readonly ICookingApparatusServices _cookingApparatusServices;

    public FoodRepository(ICookingApparatusServices cookingApparatusServices)
    {
        _cookingApparatusServices = cookingApparatusServices;
        _foods = new List<Food>();
    }

    public async Task GenerateMenu()
    {
        var oven = await _cookingApparatusServices.GetByName("Oven");
        var stove = await _cookingApparatusServices.GetByName("Stove");

        _foods.Add(new Food(1, "Pizza", 20, 2, oven));
        _foods.Add(new Food(2, "Salad", 10, 1, null));
        _foods.Add(new Food(3, "Zeama", 7, 1, stove));
        _foods.Add(new Food(4, "Scallop Sashimi with Meyer Lemon Confit", 32, 3, null));
        _foods.Add(new Food(5, "Island Duck with Mulberry Mustard", 35, 3, oven));
        _foods.Add(new Food(6, "Waffles", 10, 1, stove));
        _foods.Add(new Food(7, "Aubergine", 20, 2, oven));
        _foods.Add(new Food(8, "Lasagna", 30, 2, oven));
        _foods.Add(new Food(9, "Burger", 15, 1, stove));
        _foods.Add(new Food(10, "Gyros", 15, 1, null));
        _foods.Add(new Food(11, "Kebab", 15, 1, null));
        _foods.Add(new Food(12, "UnagiMaki", 20, 2, null));
        _foods.Add(new Food(13, "TobaccoChicken", 30, 2, oven));
    }

    public Task<IList<Food>> GetAll()
    {
        return Task.FromResult(_foods);
    }

    public Task<Food?> GetById(int id)
    {
        return Task.FromResult(_foods.FirstOrDefault(t => t.Id.Equals(id)));
    }

    public async Task<IList<Food>> GetFoodFromOrder(IEnumerable<int> foodIds)
    {
        var foods = new List<Food>();
        foreach (var id in foodIds)
        {
            var food = await GetById(id);
            if (food != null)
            {
                foods.Add(food);
            }
        }

        return foods;
    }
}