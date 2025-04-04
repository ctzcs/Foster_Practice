using System.Numerics;
using Arch.AOT.SourceGenerator;
using Foster.Framework;

namespace Engine.Source.Render;
[Component]
public struct SpriteRenderer:IRenderer
{
    public Texture texture;
    public Color color;
    public void Draw(Batcher batcher,in Vector2 worldPosition)
    {
        batcher.Image(texture, worldPosition, color);
    }
}