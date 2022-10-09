using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Services.CookingApparatusServices;

public interface ICookingApparatusServices
{
    Task GenerateCookingApparatus();
    Task<ConcurrentBag<CookingApparatus>> GetAll();
    Task<CookingApparatus> GetByName(string name);
}