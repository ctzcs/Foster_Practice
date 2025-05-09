﻿using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Performance;
using Engine.Transform;
using Foster.Framework;
using Vector2 = System.Numerics.Vector2;

namespace Engine.Camera;

public partial class CameraSystem:BaseSystem<World,float>
{
    private World world;
    private App ctx;
    private float speed;
    private float scaleSpeed;
    private float deltaTime;
    public static Entity Camera;
    private Target target;
    public CameraSystem(World world,App ctx,Target target) : base(world)
    {
        this.world = world;
        this.ctx = ctx;
        speed = 1000;
        scaleSpeed = 10;
        this.target = target;
    }

    public override void Initialize()
    {
        var ent = CameraExt.CreateCamera(target,world,0,Vector2.One);
        Camera = ent;
        ctx.Window.OnResize += OnResize;
    }

    public override void Dispose()
    {
        ctx.Window.OnResize -= OnResize;
    }

    public override void Update(in float t)
    {
#if DEBUG
        using var zone = Profiler.BeginZone(nameof(CameraSystem));
#endif
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


    void OnResize()
    {
        //TODO 除了直接Resize之外，还可以调控camera的缩放比例
        CameraExt.ResizeCamera(Camera,ctx.Window);
    }
}