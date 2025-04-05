using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.AOT.SourceGenerator;
using Foster.Framework;

namespace Engine.Source.Render;
[Component]
public struct SpriteRenderer
{
    public Texture texture;
    public Color color;
    /// <summary>
    /// 通过Texture的width/2和height/2找到中心点
    /// </summary>
    public Vector2 origin;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Draw(Batcher batcher,in Transform.Transform transform)
    {
        batcher.Image(texture,transform.position,origin, transform.scale,transform.rad,color);
    }
}