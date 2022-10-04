using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Repositories.OrderHistoryRepository;

public class OrderHistoryRepository : IOrderHistoryRepository
{
    private readonly ConcurrentBag<OrderHistory> _orderHistories;

    public OrderHistoryRepository()
    {
        _orderHistories = new ConcurrentBag<OrderHistory>();
    }

    public ConcurrentBag<OrderHistory> GetAll()
    {
        return _orderHistories;
    }

    public void Insert(OrderHistory orderHistory)
    {
        _orderHistories.Add(orderHistory);
    }
}