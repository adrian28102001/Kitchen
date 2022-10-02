using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Kitchen.Models;
using Kitchen.Repositories.GenericRepository;

namespace Kitchen.Repositories.OrderRepository;

public class OrderRepository : IOrderRepository
{
    public ObservableCollection<Order> Orders { get; set; }
    private readonly INotifyPropertyChanged _eventList;
    
    public OrderRepository()
    {
        _eventList = new ObservableCollection<Order>();
        Orders = new ObservableCollection<Order>();
    }

    public Task InsertOrder(Order order)
    {
        Orders.Add(order);
        return Task.CompletedTask;
    }
    
    public Task<List<Order>> GetOldestOrders()
    {
        return Task.FromResult(Orders.OrderBy(o => o.CreatedOnUtc).ToList());
    }

    public Task<Order?> GetOrderToPrepare()
    {
        return Task.FromResult(Orders.OrderBy(o => o.CreatedOnUtc).ThenBy(o => o.Priority).FirstOrDefault());
    }

    public Task<ObservableCollection<Order>> GetAll()
    {
        return Task.FromResult(Orders);
    }
    
    public Task<Order?> GetById(int id)
    {
        return Task.FromResult(Orders.FirstOrDefault(order => order.Id.Equals(id)));
    }
}