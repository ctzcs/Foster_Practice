using System.Numerics;
using Foster.Framework;

namespace Engine.Render;

public interface IRenderer
{
    void Draw(Batcher batcher,in Vector2 worldPosition);
}