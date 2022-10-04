namespace Kitchen.Models;

public class OrderHistory : BaseEntity
{
    public int OrderId { get; set; }
    public int FoodId { get; set; }
    public int CookerId { get; set; }
}