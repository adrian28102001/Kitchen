namespace Kitchen.Models;

public class Food : BaseEntity
{
    public Food(int id, string name, int preparationTime, int complexity, CookingApparatus? cookingApparatus)
    {
        Id = id;
        Name = name;
        PreparationTime = preparationTime;
        Complexity = complexity;
        CookingApparatus = cookingApparatus;
    }

    public string Name { get; set; }
    public int PreparationTime { get; set; }
    public int Complexity { get; set; }
    public CookingApparatus? CookingApparatus { get; set; }
    public bool IsReady { get; set; }
}