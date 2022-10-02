using Kitchen.Models;

namespace Kitchen.Repositories.FoodRepository;

public interface IFoodRepository
{
    Task GenerateMenu();
    public Task<IList<Food>> GetAll();
    public Task<Food?> GetById(int id);
    Task<IList<Food>> GetFoodFromOrder(IEnumerable<int> foodIds);
}