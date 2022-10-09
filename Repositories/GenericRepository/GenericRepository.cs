using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Repositories.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ConcurrentBag<T> _repository;

    public GenericRepository(ConcurrentBag<T> repository)
    {
        _repository = repository;
    }

    public Task<ConcurrentBag<T>> GetAll()
    {
        return Task.FromResult(_repository);
    }

    public Task<T> GetById(int id)
    {
        return Task.FromResult(_repository.FirstOrDefault(t => t.Id.Equals(id)))!;
    }

}