

using Content;

static class Program
{
    public static void Main(string[] args)
    {
        using GameContent gameContent = new GameContent(new(
            ApplicationName: "Game",
            WindowTitle: "Game",
            Width: 1280,
            Height: 720));
        gameContent.Run();
    }
    
}

