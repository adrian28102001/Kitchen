namespace Kitchen.Models;

public enum OrderStatus
{
    IsAvailable = 1,
    ReadyToOrder = 2,
    OrderTaken = 3,
    ReceivedOrder = 4,
    WaitingForWaiter = 5,
    ReadyToBeServed
}