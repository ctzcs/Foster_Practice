using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Transform;
using Foster.Framework;
using Vector2 = System.Numerics.Vector2;

namespace Engine.Source.Camera;

public partial class CameraMoveSystem:BaseSystem<World,float>
{
    private World world;
    private App ctx;
    private float speed;
    private float scaleSpeed;
    private float deltaTime;
    public static Entity Camera;
    public CameraMoveSystem(World world,App ctx) : base(world)
    {
        this.world = world;
        this.ctx = ctx;
        speed = 1000;
        scaleSpeed = 10;
    }

    public override void Initialize()
    {
        var ent = CameraExt.CreateCamera(ctx.Window,world,0,Vector2.One);
        Camera = ent;
    }


    public override void Update(in float t)
    {
        deltaTime = t;
        MoveCameraQuery(world);
    }

    [Query]
    [All<Camera2D,Transform.Transform>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MoveCamera(ref Transform.Transform transform,ref Camera2D camera)
    {
        if (ctx.Input.Keyboard.PressedOrRepeated(Keys.Right) 
            || ctx.Input.Keyboard.PressedOrRepeated(Keys.D))
        {
            transform.localPosition.X += deltaTime * speed;
            transform.SetLocalPosition(transform.localPosition);
        }else if (ctx.Input.Keyboard.PressedOrRepeated(Keys.Left)
                  ||ctx.Input.Keyboard.PressedOrRepeated(Keys.A))
        {
            transform.localPosition.X -= deltaTime * speed;
            transform.SetLocalPosition(transform.localPosition);
        }
        
        if (ctx.Input.Keyboard.PressedOrRepeated(Keys.Up)
            ||ctx.Input.Keyboard.PressedOrRepeated(Keys.W))
        {
            transform.localPosition.Y -= deltaTime * speed;
            transform.SetLocalPosition(transform.localPosition);
        }else if (ctx.Input.Keyboard.PressedOrRepeated(Keys.Down)
                  ||ctx.Input.Keyboard.PressedOrRepeated(Keys.S))
        {
            transform.localPosition.Y += deltaTime * speed;
            transform.SetLocalPosition(transform.localPosition);
        }

        if (ctx.Input.Mouse.Wheel.Y < 0)
        {
            camera.SetScaleRate(deltaTime * scaleSpeed);
        }else if (ctx.Input.Mouse.Wheel.Y > 0)
        {
            camera.SetScaleRate(-deltaTime * scaleSpeed);
        }

        if (ctx.Input.Keyboard.PressedOrRepeated(Keys.E))
        {
            transform.SetLocalRotation(transform.localRad - 1*Calc.DegToRad);
        }else if (ctx.Input.Keyboard.PressedOrRepeated(Keys.Q))
        {
            transform.SetLocalRotation(transform.localRad + 1*Calc.DegToRad);
        }
    }
}