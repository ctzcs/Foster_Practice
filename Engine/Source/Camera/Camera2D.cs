using System.Numerics;
using Arch.AOT.SourceGenerator;
using Foster.Framework;
namespace Engine.Source.Camera;
[Component]
public struct Camera2D
{
    /// <summary>
    /// 相机视口，x,y为左上角，w,h为参考的宽高
    /// </summary>
    public Rect rect;
    public float scaleRate;
    public Vector2 shake;
    
}