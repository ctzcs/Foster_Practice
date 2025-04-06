namespace Engine.Source.Utility;

public class EnginePlatform
{
    public static byte[] ReadEmbeddedBytes<T>(string name)
    {
        using (Stream? manifestResourceStream = typeof(T).Assembly.GetManifestResourceStream(name))
        {
            if (manifestResourceStream == null)
                return Array.Empty<byte>();
            byte[] buffer = new byte[manifestResourceStream.Length];
            manifestResourceStream.ReadExactly((Span<byte>) buffer);
            return buffer;
        }
    }
}