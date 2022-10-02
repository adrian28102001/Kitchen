namespace Kitchen.Helpers;

public static class SleepGenerator
{
    public static Task Delay(int sleep)
    {
        return Task.Delay(TimeSpan.FromSeconds(sleep));
    }
}