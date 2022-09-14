namespace Kitchen.Models;

public class Food : Entity
{
    public string Name { get; set; }
    public int PreparationTime { get; set; }
    public int Complexity { get; set; }
    public CookingApparatus CookingApparatus { get; set; }
    public bool IsReady { get; set; }
}