using Foster.Framework;

namespace Editor;

public class Program
{
    public static void Main(string[] args)
    {
        using var editor = new Editor(new AppConfig(
            "Editor",
            "Editor",
            1920,
            1080));
        editor.Run();
    }
}