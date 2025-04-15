using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;

using Foster.Framework;

namespace Engine.Camera;

public class CameraExt
{
    public static Entity CreateCamera(IDrawableTarget target,World world,float rotation ,Vector2 size)
    {
        var ent = world.Create(
            new Transform.Transform(Entity.Null,Vector2.Zero, rotation,size),
            new Camera2D()
            {
                rect = new Rect(0, 0, target.WidthInPixels, target.HeightInPixels),
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

    //如果是target不在视口中心，得让mouse跟target一样移动到原点，然后放缩
    public static Vector2 ScreenToViewport(Vector2 screenPosition,Window window)
    {
        return new Vector2(screenPosition.X / window.WidthInPixels, screenPosition.Y / window.HeightInPixels);
    }

    public static Vector2 ViewportToLogicScreen(Vector2 viewport,Vector2 logicScreen)
    {
        return new Vector2(logicScreen.X * viewport.X, logicScreen.Y * viewport.Y);
    }
    
    
    /// <summary>
    /// 这个不仅依赖于屏幕坐标，还依赖于比例
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static Vector2 ScreenToWorld(Vector2 screenPosition,Window window,Vector2 logicScreen)
    {
        //
        var pos = ViewportToLogicScreen(ScreenToViewport(screenPosition, window),logicScreen);
        
        
        //定camera坐标为0时的点为世界原点，
        //这个时候screenpos = width/2,height/2
        //TODO 坐标计算还是不对
        //屏幕转相机
        //相机转世界
        var camera = CameraMoveSystem.Camera;
        ref var transform = ref camera.Get<Transform.Transform>();
        ref var camera2D = ref camera.Get<Camera2D>();
        //0,0为原点,偏移相机Center

        var mat = Foster.Framework.Transform.CreateMatrix(
            camera2D.rect.Center,
            -transform.position,
            transform.scale / camera2D.scaleRate,
            -transform.rad);
        
        var screenPos = Matrix3x2.CreateTranslation(pos);
        
        Matrix3x2.Invert(mat,out var invert);
        var result = Matrix3x2.Multiply(screenPos,invert);
        return new Vector2(result.M31, result.M32);
        
    }
    
    
}