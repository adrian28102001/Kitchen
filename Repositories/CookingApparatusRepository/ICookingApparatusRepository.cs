using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Repositories.CookingApparatusRepository;

public interface ICookingApparatusRepository
{
    Task GenerateCookingApparatus();
    Task<ConcurrentBag<CookingApparatus>> GetAll();
    Task<CookingApparatus> GetByName(string name);
}