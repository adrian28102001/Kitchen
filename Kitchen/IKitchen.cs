namespace Kitchen.Kitchen;

public interface IKitchen
{
    Task InitializeKitchenParallelAsync();
    Task MaintainKitchen();
}