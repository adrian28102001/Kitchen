using System.Collections.Concurrent;
using Kitchen.Models;
using Kitchen.Repositories.OrderHistoryRepository;

namespace Kitchen.Services.OrderHistoryService;

public class OrderHistoryService : IOrderHistoryService
{
    private readonly IOrderHistoryRepository _orderHistoryRepository;

    public OrderHistoryService(IOrderHistoryRepository orderHistoryRepository)
    {
        _orderHistoryRepository = orderHistoryRepository;
    }

    public ConcurrentBag<OrderHistory> GetAll()
    {
        return _orderHistoryRepository.GetAll();
    }

    public void Insert(OrderHistory orderHistory)
    {
        _orderHistoryRepository.Insert(orderHistory);
    }
}