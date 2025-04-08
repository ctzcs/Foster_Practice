using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Engine.Source.Render;
using Foster.Framework;

namespace Engine.Source.Camera;

public class CameraExt
{
    public static Entity CreateCamera(Window window,World world,float rotation ,Vector2 size)
    {
        var ent = world.Create(
            new Transform.Transform(Entity.Null,Vector2.Zero, rotation,size),
            new Camera2D()
            {
                rect = new Rect(0, 0, window.WidthInPixels, window.HeightInPixels),
                shake = Vector2.Zero,
                scaleRate = 1,
            });
        
        return ent;
    }
    
    
    public static Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        //定camera坐标为0时的点为世界原点，
        //这个时候screenpos = width/2,height/2
        //TODO 坐标计算还是不对
        //屏幕转相机
        //相机转世界
        var camera = CameraMoveSystem.Camera;
        ref var transform = ref camera.Get<Transform.Transform>();
        ref var camera2D = ref camera.Get<Camera2D>();
        //0,0为原点,偏移相机Center
        
        
        var v1 = Matrix3x2.CreateTranslation(screenPosition  + transform.position - camera2D.rect.Center);
        
        var v2 = Matrix3x2.Multiply(v1, Matrix3x2.CreateRotation(-transform.rad));

        var v3 = Matrix3x2.Multiply(v2,Matrix3x2.CreateScale(Vector2.One * camera2D.scaleRate));
        
        return new Vector2(v3.M31, v3.M32);
        
    }

    /*public static Vector2 WorldToScreen()
    {
        
    }
    */
    
}