using Newtonsoft.Json;

namespace Kitchen.Models;

public class Order : Entity
{
    public int TableId { get; set; }
    public int WaiterId { get; set; }
    public int Priority { get; set; }
    public int MaxWait { get; set; }
    public bool OrderIsComplete { get; set; }
    public List<int> FoodList { get; set; }
    [JsonIgnore] public OrderStatus OrderStatus { get; set; }
}