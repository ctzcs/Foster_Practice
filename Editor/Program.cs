namespace Editor;

public class Program
{
    public static void Main(string[] args)
    {
        using Editor editor = new Editor(new(
            ApplicationName: "Editor",
            WindowTitle: "Editor",
            Width: 1920,
            Height: 1080));
        editor.Run();
    }
}