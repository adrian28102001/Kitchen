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
        _cookingApparatus.Add(new CookingApparatus()
        {
            Id = 1,
            Name = "Stove",
            IsBusy = false
        });
        _cookingApparatus.Add(new CookingApparatus()
        {
            Id = 2,
            Name = "Oven",
            IsBusy = false
        });
        _cookingApparatus.Add(new CookingApparatus()
        {
            Id = 3,
            Name = "Oven",
            IsBusy = false
        });
        return Task.CompletedTask;
    }

    public Task<ConcurrentBag<CookingApparatus>> GetAll()
    {
        return _genericRepository.GetAll();
    }
}