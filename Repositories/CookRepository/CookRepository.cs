using System.Collections.Concurrent;
using Kitchen.Models;
using Kitchen.Repositories.GenericRepository;

namespace Kitchen.Repositories.CookRepository;

public class CookRepository : ICookRepository
{
    private readonly ConcurrentBag<Cook> _cooks;
    private readonly IGenericRepository<Cook> _genericRepository;

    public CookRepository()
    {
        _cooks = new ConcurrentBag<Cook>();
        _genericRepository = new GenericRepository<Cook>(_cooks);
    }

    public Task<ConcurrentBag<Cook>> GetAll()
    {
        return _genericRepository.GetAll();
    }

    public Task GenerateCooks()
    {
        _cooks.Add(new Cook
        {
            Id = 1,
            Name = "Cook1",
            Rank = 3,
            Proficiency = 4,
            CatchPhrase = "Cook1",
            IsBusy = false,
            CookingList = new List<Food>()
        });
        _cooks.Add(new Cook
        {
            Id = 2,
            Name = "Cook2",
            Rank = 2,
            Proficiency = 3,
            CatchPhrase = "Cook2",
            IsBusy = false,
            CookingList = new List<Food>()
        });
        _cooks.Add(new Cook
        {
            Id = 3,
            Name = "Cook3",
            Rank = 2,
            Proficiency = 2,
            CatchPhrase = "Cook3",
            IsBusy = false,
            CookingList = new List<Food>()
        });
        _cooks.Add(new Cook
        {
            Id = 4,
            Name = "Cook4",
            Rank = 1,
            Proficiency = 2,
            CatchPhrase = "Cook4",
            IsBusy = false,
            CookingList = new List<Food>()
        });
        return Task.CompletedTask;
    }

    public Task<Cook> GetById(int id)
    {
        return _genericRepository.GetById(id);
    }

    public Task<Cook?> GetFreeCook()
    {
        return Task.FromResult(_cooks.FirstOrDefault(cook => cook.IsBusy.Equals(false)));
    }

    public Task<Cook> GetCookerByRank(int rank)
    {
        return Task.FromResult(_cooks.LastOrDefault(cook => cook.Rank.Equals(rank)))!;
    }

    public Task<Cook?> GetSpecialCooker(int rank, int proficiency)
    {
        return Task.FromResult(_cooks
            .First(cook => cook.Rank.Equals(rank) && cook.Proficiency.Equals(proficiency)))!;
    }
}