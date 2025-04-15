using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Engine.Camera;
using Engine.Render;
using Foster.Framework;

namespace Content.Test;

public partial class StateSystem:BaseSystem<World,float>
{
    private World world;
    private App ctx;
    private Resources res;
    private FrameCounter frameCounter;
    private float stateEase = 1;
    private EState state;
    private static int count = 0;
    
    public Entity line = Entity.Null;
    
    public enum EState
    {
        Frog,
        Line,
        Building,
    }
    
    public StateSystem(World world,App app,Resources res,FrameCounter frameCounter) : base(world)
    {
        this.world = world;
        this.ctx = app;
        this.res = res;
        this.frameCounter = frameCounter;
    }

    public override void Initialize()
    {
        state = EState.Frog;
    }


    public override void Update(in float t)
    {
        
        switch (state)
        {
            case EState.Frog:
                if (ctx.Input.Keyboard.Pressed(Keys.Space))
                {
                    state = EState.Line;
                }
                
                if (ctx.Input.Mouse.LeftDown)
                {
                    var pos = CameraExt.ScreenToWorld(ctx.Input.Mouse.Position,ctx.Window,res.logicSize);
                    //TestExt.CreateSimpleFrog(world, pos,0,Vector2.One,texture, Color.Red);
                    var frog = Engine.Asset.Assets.GetSubtexture("frog/0");
                    TestExt.CreateSimpleFrog(world, pos,0,Vector2.One,frog , Color.Black,0);
                    count++;
                }
                
                break;
            case EState.Line:
                if (ctx.Input.Keyboard.Pressed(Keys.Space))
                {
                    state = EState.Building;
                }
                
                if (ctx.Input.Mouse.LeftPressed)
                {
                    if (line == Entity.Null)
                    {
                        line = TestExt.CreatLine(world,Vector2.Zero,0,Vector2.One,Color.Gray,5);
                    }
                    ref var render = ref line.Get<LineRenderer>();
                    render.AddPoint(CameraExt.ScreenToWorld(ctx.Input.Mouse.Position,ctx.Window,res.logicSize));
                }
                else if (ctx.Input.Mouse.RightPressed)
                {
                    if (line != Entity.Null)
                    {
                        line.Get<LineRenderer>().RemoveLast();
                    }
                    
                }else if (ctx.Input.Keyboard.Pressed(Keys.S))
                {
                    line = Entity.Null;
                }
                
                break;
            case EState.Building:
                if (ctx.Input.Keyboard.Pressed(Keys.Space))
                {
                    state = EState.Frog;
                }
                
                if (ctx.Input.Mouse.LeftDown)
                {
                    var pos = CameraExt.ScreenToWorld(ctx.Input.Mouse.Position,ctx.Window,res.logicSize);
                    //TestExt.CreateSimpleFrog(world, pos,0,Vector2.One,texture, Color.Red);
                    var building = Engine.Asset.Assets.GetSubtexture("bd/0");
                    TestExt.CreateBuilding(world, Vector2.Zero, 0,Vector2.One * 5, building, Color.Red,1);
                    
                    TestExt.CreateBuilding(world, ctx.Window.Size/2, 0,Vector2.One * 5, building, Color.Blue,1);
                    
                }
                break;
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
            case EState.Line:
                res.batcher.Text(res.font,"left mouse click add point,s cut down line",new Vector2(8,40),color:Color.Black);
                break;
            case EState.Frog:
                res.batcher.Text(res.font,"left mouse press add frog",new Vector2(8,40),color:Color.Black);
                break;
            case EState.Building:
                res.batcher.Text(res.font,"left mouse press add building",new Vector2(8,40),color:Color.Black);
                break;
        }
        res.batcher.CircleLine(Vector2.Zero,20,3,10,Color.Black);
        res.batcher.CircleLine(ctx.Window.SizeInPixels/2f,20,3,10,Color.Red);
        res.batcher.CircleLine(ctx.Window.SizeInPixels,20,3,10,Color.Black);
        res.batcher.Render(res.target);
        res.batcher.Clear();
    }
}