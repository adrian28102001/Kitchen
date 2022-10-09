using System.Collections.Concurrent;
using Kitchen.Models;
using Kitchen.Repositories.CookingApparatusRepository;

namespace Kitchen.Services.CookingApparatusServices;

public class CookingApparatusServices : ICookingApparatusServices
{
    private readonly ICookingApparatusRepository _cookingApparatus;

    public CookingApparatusServices(ICookingApparatusRepository cookingApparatus)
    {
        _cookingApparatus = cookingApparatus;
    }

    public Task GenerateCookingApparatus()
    {
        return _cookingApparatus.GenerateCookingApparatus();
    }

    public Task<ConcurrentBag<CookingApparatus>> GetAll()
    {
        return _cookingApparatus.GetAll();
    }

    public Task<CookingApparatus> GetByName(string name)
    {
        return _cookingApparatus.GetByName(name);
    }
}