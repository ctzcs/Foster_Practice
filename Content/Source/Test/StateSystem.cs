using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Engine.Source.Camera;
using Engine.Source.Render;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public partial class StateSystem:BaseSystem<World,float>
{
    private World world;
    private App ctx;
    private Resources res;
    private FrameCounter frameCounter;
    private float stateEase = 1;
    private string state;
    private static int count = 0;
    
    public Entity line = Entity.Null;
    
    public StateSystem(World world,App app,Resources res,FrameCounter frameCounter) : base(world)
    {
        this.world = world;
        this.ctx = app;
        this.res = res;
        this.frameCounter = frameCounter;
    }

    public override void Initialize()
    {
        state = "Frog";
    }


    public override void Update(in float t)
    {
        if (ctx.Input.Keyboard.Pressed(Keys.Space))
        {
            switch (state)
            {
                case "Frog":
                    state = "Line";
                    break;
                case "Line":
                    state = "Frog";
                    break;
            }
        }

        if (ctx.Input.Mouse.LeftDown)
        {
            switch (state)
            {
                case "Frog":
                    var pos = CameraExt.ScreenToWorld(ctx.Input.Mouse.Position);
                    //TestExt.CreateSimpleFrog(world, pos,0,Vector2.One,texture, Color.Red);
                    TestExt.CreateFrogCarrier(world, pos,0,Vector2.One*2, res.texture, Color.Red,5);
                    count++;
                    break;
            }
        }
        
        if (ctx.Input.Mouse.LeftPressed)
        {
            switch (state)
            {
                case "Line":
                    if (line == Entity.Null)
                    {
                        line = TestExt.CreatLine(world,Vector2.Zero,0,Vector2.One,Color.Gray,5);
                    }
                    line.Get<LineRenderer>().AddPoint(CameraExt.ScreenToWorld(ctx.Input.Mouse.Position));
                    break;
            }
        }
        else if (ctx.Input.Mouse.RightPressed)
        {
            switch (state)
            {
                case "Line":
                    line.Get<LineRenderer>().RemoveLast();
                    break;
            }
        }else if (ctx.Input.Keyboard.Pressed(Keys.S))
        {
            switch (state)
            {
                case "Line":
                    line = Entity.Null;
                    break;
            }
        }
    }

    public override void AfterUpdate(in float t)
    {
        frameCounter.Update();
        res.batcher.Clear();
        res.batcher.Quad(new Quad(new Vector2(0,0),new Vector2(600,0),new Vector2(600,100),new Vector2(0,100)),Color.Green);
        res.batcher.Text(res.font, $"Frog Group Count:{count} {frameCounter.FPS} FPS", new Vector2(8, 0), Color.Black);
        res.batcher.Text(res.font, $"State:{state},Press Space To Change", new Vector2(8,20), Color.Black);
        switch (state)
        {
            case "Line":
                res.batcher.Text(res.font,"left mouse click add point,s cut down line",new Vector2(8,40),color:Color.Black);
                break;
            case "Frog":
                res.batcher.Text(res.font,"left mouse press add frog",new Vector2(8,40),color:Color.Black);
                break;
        }
        res.batcher.Render(ctx.Window);
        res.batcher.Clear();
    }
}