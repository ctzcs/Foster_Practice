using Foster.Framework;

namespace Editor;

public class Program
{
    public static void Main(string[] args)
    {
        using var editor = new Editor();
        editor.Run();
    }
}