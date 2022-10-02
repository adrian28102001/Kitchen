using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Kitchen.Models;

namespace Kitchen.Services.OrderService;

public interface IOrderService
{
    Task InsertOrder(Order order);
    Task<ObservableCollection<Order>> GetAll();
    Task SendOrder(Order order);
    void PrepareOrder(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);
}