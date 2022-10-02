namespace Kitchen.Helpers;

public static class ConsoleHelper
{
    public static void Print (string message)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(message);
    }
    public static void Print (string message, ConsoleColor consoleColor)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message);
    }
}