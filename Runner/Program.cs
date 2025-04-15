using Content;
using Foster.Framework;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var gameContent = new GameApp(new AppConfig(
            "Game",
            "Game",
            1280,
            720,Flags:AppFlags.EnableGraphicsDebugging));
        gameContent.Run();
    }
}