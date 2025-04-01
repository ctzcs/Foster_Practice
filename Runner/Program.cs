using Content;
using Foster.Framework;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var gameContent = new GameContent(new AppConfig(
            "Game",
            "Game",
            1280,
            720));
        gameContent.Run();
    }
}