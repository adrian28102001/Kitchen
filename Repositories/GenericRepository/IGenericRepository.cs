using System.Collections.Concurrent;

namespace Kitchen.Repositories.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<ConcurrentBag<T>> GetAll();
    Task<T> GetById(int id);
}