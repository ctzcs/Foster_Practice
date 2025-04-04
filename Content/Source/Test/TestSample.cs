using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Content.Test;
using Engine.Source;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;


namespace Content;

public class TestSample:ILifetime
{
    private readonly App ctx;
    private readonly World world;
    private readonly Group<float> modules;
    private Rng rng = new(1337);
    private readonly SpriteFont font;
    private readonly Texture texture;
    private Batcher batcher;
    
    private FrameCounter frameCounter;
    private float deltaTime = 0;
    private const int IterCount = 1000;
    private static int count = 0;

    private string state;
    public Entity line = Entity.Null;
    public TestSample(App ctx)
    {
        this.ctx = ctx;
        world = World.Create();
        batcher = new Batcher(this.ctx.GraphicsDevice);
        font = new SpriteFont(ctx.GraphicsDevice, Path.Join("Assets", "Fonts","monogram.ttf"), 32);
        texture = new Texture(ctx.GraphicsDevice, new Image(Path.Join("Assets","Sprites", "frog_knight.png")));
        modules = new Group<float>("TestGroup",
            new RandomPositionSystem(world,rng),
            new FindLineSystem(world,rng),
            new TransformSystem(world),
            new RenderSystem(world,batcher,ctx.Window));

        frameCounter = new FrameCounter();
    }
    

    public void Start()
    {
        state = "Frog";
    }

    public void Destroy()
    {
    }

    public void Update()
    {
        deltaTime = ctx.Time.Delta;
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
                    var pos = ctx.Input.Mouse.Position;
                    var e = TestExt.CreateSimpleFrog(world, pos, Vector2.One, texture, Color.Red, 0);
                    var e2 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Green, 1);
                    var e3 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        2);
                    var e4 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        3);
                    var e5 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        4);
                    var e6 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        5);
                    var e7 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        6);
                    var e8 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        7);
                    var e9 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        8);
                    var e10 = TestExt.CreateSimpleFrog(world, new Vector2(0, -10), Vector2.One, texture, Color.Yellow,
                        9);
                    e2.SetParent(e);
                    e3.SetParent(e2);
                    e4.SetParent(e3);
                    e5.SetParent(e4);
                    e6.SetParent(e5);
                    e7.SetParent(e6);
                    e8.SetParent(e7);
                    e9.SetParent(e8);
                    e10.SetParent(e9);
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
                        line = TestExt.CreatLine(world,Vector2.Zero,Vector2.One,Color.Gray,5);
                        
                    }
                    line.Get<LineRenderer>().AddPoint(ctx.Input.Mouse.Position);
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
        
        modules.Update(in deltaTime);
    }

    public void Render()
    {
        frameCounter.Update();
        ctx.Window.Clear(Color.White);
        modules.AfterUpdate(in deltaTime);
        batcher.Render(ctx.Window);
        batcher.Clear();
        batcher.Quad(new Quad(new Vector2(0,0),new Vector2(600,0),new Vector2(600,100),new Vector2(0,100)),Color.Green);
        batcher.Text(font, $"Frog Group Count:{count} {frameCounter.FPS} FPS", new Vector2(8, 0), Color.Black);
        batcher.Text(font, $"State:{state},Press Space To Change", new Vector2(8,20), Color.Black);
        switch (state)
        {
            case "Line":
                batcher.Text(font,"left mouse click add point,s cut down line",new Vector2(8,40),color:Color.Black);
                break;
            case "Frog":
                batcher.Text(font,"left mouse press add frog",new Vector2(8,40),color:Color.Black);
                break;
        }
        
        batcher.Render(ctx.Window);
        batcher.Clear();
    }


    
    
}