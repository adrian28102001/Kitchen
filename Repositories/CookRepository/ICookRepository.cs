using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Repositories.CookRepository;

public interface ICookRepository
{
    Task GenerateCooks();
    Task<ConcurrentBag<Cook>> GetAll();
    Task<Cook> GetById(int id);
    Task<Cook?> GetFreeCook();
    Task<Cook> GetCookerByRank(int rank);
    Task<Cook?> GetSpecialCooker(int rank, int proficiency);
}