using System.Collections.ObjectModel;
using Kitchen.Models;

namespace Kitchen.Repositories.OrderRepository;

public interface IOrderRepository
{
    ObservableCollection<Order> Orders { get; set; }
    Task<ObservableCollection<Order>> GetAll();
    Task<Order?> GetById(int id);
    Task InsertOrder(Order order);
    Task<List<Order>> GetOldestOrders();
    Task<List<Order>> GetOrdersToPrepare();
}