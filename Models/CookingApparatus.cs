namespace Kitchen.Models;

public class CookingApparatus : Entity
{
    public int Name { get; set; }
    public int EstimateTime { get; set;}
    public bool IsBusy { get; set;}

}