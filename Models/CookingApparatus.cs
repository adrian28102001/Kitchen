namespace Kitchen.Models;

public class CookingApparatus : BaseEntity
{
    public CookingApparatus()
    {
        
    }
    public CookingApparatus(int id, string name,bool isBusy)
    {
        Id = id;
        Name = name;
        IsBusy = isBusy;
    }

    public string Name { get; set; }
    public int EstimateTime { get; set;}
    public bool IsBusy { get; set;}

}