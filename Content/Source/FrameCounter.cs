using System.Diagnostics;

namespace Content;

/// <summary>
///     Simple utility to count frames in last second
/// </summary>
public class FrameCounter
{
    public int FPS;
    public int Frames;
    public Stopwatch sw = Stopwatch.StartNew();

    public void Update()
    {
        Frames++;
        var elapsed = sw.Elapsed.TotalSeconds;
        if (elapsed > 1)
        {
            sw.Restart();
            FPS = Frames;
            Frames = 0;
        }
    }
}