using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Services.OrderHistoryService;

public interface IOrderHistoryService
{
    ConcurrentBag<OrderHistory> GetAll();
    void Insert(OrderHistory orderHistory);
}