using Content;
using Foster.Framework;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var gameContent = new GameApp(new AppConfig(
            "Game",
            "Game",
            1920,
            1080));
        gameContent.Run();
    }
}