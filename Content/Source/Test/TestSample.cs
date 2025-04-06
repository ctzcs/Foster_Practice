using Arch.Core;
using Arch.System;
using Engine.Source;
using Engine.Source.Camera;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;


namespace Content.Test;

public class TestSample:ILifetime
{
    private readonly App ctx;
    private readonly World world;
    private readonly Group<float> modules;
    private Rng rng = new(1337);
    private Resources res;
    private FrameCounter frameCounter;
    private float deltaTime = 0;
    
    public TestSample(App ctx)
    {
        this.ctx = ctx;
        world = World.Create();
        res = new Resources(
            new SpriteFont(ctx.GraphicsDevice, Path.Join("Assets", "Fonts","monogram.ttf"), 32),
            new Texture(ctx.GraphicsDevice, new Image(Path.Join("Assets","Sprites", "frog_knight.png"))),
            new Batcher(this.ctx.GraphicsDevice));
        frameCounter = new FrameCounter();
        modules = new Group<float>("TestGroup",
            new StateSystem(world,ctx,res,frameCounter),
            new RandomPositionSystem(world,rng),
            new FindLineSystem(world,rng),
            new CameraMoveSystem(world,ctx),
            new TransformSystem(world),
            new RenderSystem(world,res.batcher,ctx.Window));
        
    }
    

    public void Start()
    {
        modules.Initialize();
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
        
        ctx.Window.Clear(Color.White);
        modules.AfterUpdate(in deltaTime);
        res.batcher.Render(ctx.Window);
        res.batcher.Clear();
        //文字显示
        
    }
}


