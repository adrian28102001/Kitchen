namespace Kitchen.Models;

public class CookingApparatus : BaseEntity
{
    public string Name { get; set; }
    public int EstimateTime { get; set;}
    public bool IsBusy { get; set;}

}