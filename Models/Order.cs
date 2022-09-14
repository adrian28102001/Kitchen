using System.ComponentModel.DataAnnotations;

namespace Kitchen.Models;

public class Order : Entity
{
    public int TableId { get; set; }
    public int WaiterId { get; set; }
    [Range(1, 3)] public int Priority { get; set; }
    public int MaxWait { get; set; }
    public bool OrderIsComplete { get; set; }
    public IList<int> FoodList { get; set; }
    public Status Status { get; set; } 
}