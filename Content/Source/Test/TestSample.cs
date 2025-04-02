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
    public TestSample(App ctx)
    {
        this.ctx = ctx;
        world = World.Create();
        batcher = new Batcher(this.ctx.GraphicsDevice);
        font = new SpriteFont(ctx.GraphicsDevice, Path.Join("Assets", "monogram.ttf"), 32);
        texture = new Texture(ctx.GraphicsDevice, new Image(Path.Join("Assets", "frog_knight.png")));
        modules = new Group<float>("TestGroup",
            new RandomPosition(world,rng),
            new TransformSystem(world),
            new RenderSystem(world,batcher,ctx.Window));

        frameCounter = new FrameCounter();
    }
    

    public void Start()
    {
        for (int i = 0; i < 20000; i++)
        {
            var e = TestExt.CreateSimpleFrog(world,new Vector2(rng.Float(-100,1500),rng.Float(-100,1000)),Vector2.One,texture,Color.Green);
            var e2 = TestExt.CreateSimpleFrog(world,new Vector2(0,rng.Float(5,10)),Vector2.One,texture,Color.Green);
            ref var t = ref e.Get<Transform>();
            ref var t2 = ref e2.Get<Transform>();
            t.children.Add(e2);
            t2.parent = e;
            e2.Add<ChildOf>();
            e.Add<ParentOf>(); 
        }
        
    }

    public void Destroy()
    {
    }

    public void Update()
    {
        deltaTime = ctx.Time.Delta;
        
        modules.Update(in deltaTime);
    }

    public void Render()
    {
        frameCounter.Update();
        ctx.Window.Clear(Color.White);
        modules.AfterUpdate(in deltaTime);
        batcher.Render(ctx.Window);
        batcher.Clear();
        batcher.Text(font, $"{frameCounter.FPS} FPS", new Vector2(8, -2), Color.Black);
        batcher.Render(ctx.Window);
        batcher.Clear();
    }
    
}