using System.Numerics;
using Arch.AOT.SourceGenerator;
using Foster.Framework;
namespace Engine.Camera;
[Component]
public struct Camera2D
{
    /// <summary>
    /// 相机视口，x,y为左上角，w,h为参考的宽高
    /// </summary>
    public Rect rect;
    public float scaleRate;
    public Vector2 shake;
    
    public void SetScaleRate(float scaleRateChange)
    {
        scaleRate += scaleRateChange;
        scaleRate = Calc.Clamp(scaleRate, 0.1f, 10f);
    }
    
}


//TODO Camera2D 还是有问题，打算抄一个Camera,然后改写，看能不能用