using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.AOT.SourceGenerator;
using Foster.Framework;

namespace Engine.Render;
[Component]
public struct SpriteRenderer
{
    public Subtexture subtexture;
    public Color color;
    /// <summary>
    /// 通过Texture的width/2和height/2找到中心点
    /// </summary>
    public Vector2 origin;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Draw(Batcher batcher,in Transform.Transform transform)
    {
        batcher.Image(subtexture,transform.position,origin, transform.scale,transform.rad,color);
    }
}