namespace Kitchen.Models;

public class Cooks : Entity
{
    public string Name { get; set; }
    public int Rank { get; set; }
    public int Proficiency { get; set; }   
    public string CatchPhrase { get; set; }
    public bool IsBusy { get; set; }
}   