
using Arch.Core;
using Arch.LowLevel;
using Arch.System;
using Engine;
using Engine.Camera;
using Engine.Render;
using Engine.Scene;
using Engine.Transform;
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
        Engine.Asset.Assets.Load(ctx.GraphicsDevice);
        res = new Resources(
            Engine.Asset.Assets.Font,
            Engine.Asset.Assets.Atlas,
            new Batcher(this.ctx.GraphicsDevice));
        frameCounter = new FrameCounter();
        modules = new Group<float>("TestGroup",
            new StateSystem(world,ctx,res,frameCounter),
            new BuildingCatchSystem(world),
            
            
            
            new RandomPositionSystem(world,rng),
            new FindLineSystem(world,rng),
            
            
            
            new CameraMoveSystem(world,ctx),
            new TransformSystem(world),
            
            
            new RenderSystem(world,res.batcher,ctx.Window),
            new DestroyNotActiveEntitySystem(world));
            
            
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


