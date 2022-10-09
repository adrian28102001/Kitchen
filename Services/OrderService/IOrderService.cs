using System.Collections.ObjectModel;
using Kitchen.Models;

namespace Kitchen.Services.OrderService;

public interface IOrderService
{
    Task InsertOrder(Order order);
    Task<ObservableCollection<Order>> GetAll();
    Task PrepareOrder();
}