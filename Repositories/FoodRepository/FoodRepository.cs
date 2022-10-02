using Kitchen.Models;

namespace Kitchen.Repositories.FoodRepository;

public class FoodRepository : IFoodRepository
{
    private readonly IList<Food> _foods;

    public FoodRepository()
    {
        _foods = new List<Food>();
    }

    public Task GenerateMenu()
    {
        _foods.Add(new Food
        {
            Id = 1,
            Name = "Pizza",
            PreparationTime = 20,
            Complexity = 2
        });
        _foods.Add(new Food
        {
            Id = 2,
            Name = "Salad",
            PreparationTime = 10,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 3,
            Name = " Zeama",
            PreparationTime = 7,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 4,
            Name = "Scallop Sashimi with Meyer Lemon Confit",
            PreparationTime = 32,
            Complexity = 3
        });
        _foods.Add(new Food
        {
            Id = 5,
            Name = "Island Duck with Mulberry Mustard",
            PreparationTime = 35,
            Complexity = 3
        });
        _foods.Add(new Food
        {
            Id = 6,
            Name = "Waffles",
            PreparationTime = 10,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 7,
            Name = "Aubergine",
            PreparationTime = 20,
            Complexity = 2
        });

        _foods.Add(new Food
        {
            Id = 8,
            Name = "Lasagna",
            PreparationTime = 30,
            Complexity = 2
        });
        _foods.Add(new Food
        {
            Id = 9,
            Name = "Burger",
            PreparationTime = 15,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 10,
            Name = "Gyros",
            PreparationTime = 15,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 11,
            Name = "Kebab",
            PreparationTime = 15,
            Complexity = 1
        });
        _foods.Add(new Food
        {
            Id = 12,
            Name = "UnagiMaki",
            PreparationTime = 20,
            Complexity = 2
        });
        _foods.Add(new Food
        {
            Id = 13,
            Name = "TobaccoChicken",
            PreparationTime = 30,
            Complexity = 2
        });
        return Task.CompletedTask;
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