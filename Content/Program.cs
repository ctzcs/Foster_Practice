
namespace Content;

static class Program
{
    public static void Main(string[] args)
    {
        GameContent gameContent = new GameContent(new(
            ApplicationName: "Game",
            WindowTitle: "Game",
            Width: 1920,
            Height: 1080));
        gameContent.Run();
    }
    
}

