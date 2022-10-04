using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Repositories.OrderHistoryRepository;

public interface IOrderHistoryRepository
{
    ConcurrentBag<OrderHistory> GetAll();
    void Insert(OrderHistory orderHistory);
}
