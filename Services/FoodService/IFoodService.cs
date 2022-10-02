﻿using Kitchen.Models;

namespace Kitchen.Services.FoodService;

public interface IFoodService
{
    Task<IList<Food>> GetAll();
    Task<Food?> GetById(int id);
    Task GenerateMenu();
    Task<IList<Food>> GetFoodFromOrder(IEnumerable<int> foodIds);
    Task<IOrderedEnumerable<Food>> SortFoodByComplexity(IEnumerable<Food> foods);
}