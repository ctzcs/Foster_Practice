using System.Numerics;
using Arch.AOT.SourceGenerator;
using Foster.Framework;

namespace Engine.Source.Render;

[Component]
public struct LineRenderer:IRenderer
{
    public List<Vector2> line;
    public Color color;
    public float lineWidth;

    public void AddPoint(Vector2 point)
    {
        line.Add(point);
    }
    
    public void RemoveLast()
    {
        if (line.Count > 0)
            line.RemoveAt(line.Count - 1);
    }

    public void Draw(Batcher batcher,in Vector2 worldPosition)
    {
        for (int i = 0; i < line.Count - 1; i++)
        {
            batcher.Line(worldPosition + line[i],worldPosition + line[i + 1],lineWidth,color);
        }
    }
}