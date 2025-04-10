using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Engine.Source.Render;
using Foster.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    public static void ResizeCamera(in Entity camera,Window window)
    {
        ref var c = ref camera.Get<Camera2D>();
        c.rect = new Rect(0, 0, window.WidthInPixels, window.HeightInPixels);
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

        var mat = Foster.Framework.Transform.CreateMatrix(camera2D.rect.Center,
            -transform.position,
            transform.scale / camera2D.scaleRate,
            -transform.rad);

        var screenPos = Matrix3x2.CreateTranslation(screenPosition);
        
        Matrix3x2.Invert(mat,out var invert);
        var result = Matrix3x2.Multiply(screenPos,invert);
        return new Vector2(result.M31, result.M32);
        
    }

    /*public static Vector2 WorldToScreen()
    {
        
    }
    */
    
}