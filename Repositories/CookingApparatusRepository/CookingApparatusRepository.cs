using System.Collections.Concurrent;
using Kitchen.Models;
using Kitchen.Repositories.GenericRepository;

namespace Kitchen.Repositories.CookingApparatusRepository;

public class CookingApparatusRepository : ICookingApparatusRepository
{
    private readonly ConcurrentBag<CookingApparatus> _cookingApparatus;
    private readonly IGenericRepository<CookingApparatus> _genericRepository;

    public CookingApparatusRepository()
    {
        _cookingApparatus = new ConcurrentBag<CookingApparatus>();
        _genericRepository = new GenericRepository<CookingApparatus>(_cookingApparatus);
    }

    public Task GenerateCookingApparatus()
    {
        _cookingApparatus.Add(new CookingApparatus(1, "Stove", false));
        _cookingApparatus.Add(new CookingApparatus(2, "Oven", false));
        
        return Task.CompletedTask;
    }

    public Task<ConcurrentBag<CookingApparatus>> GetAll()
    {
        return _genericRepository.GetAll();
    }

    public Task<CookingApparatus> GetByName(string name)
    {
        return Task.FromResult(_cookingApparatus.FirstOrDefault(apparatus => apparatus.Name.Equals(name)))!;
    }
}