
using System.Numerics;
using Arch.Core;
using Arch.System;
using Engine;
using Engine.Camera;
using Engine.Performance;
using Engine.Render;
using Engine.Test;
using Engine.Transform;
using Foster.Framework;


namespace Content.Test;

public class TestSample:IContent
{
    private readonly App ctx;
    private readonly World world;
    private readonly Group<float> modules;
    private Rng rng = new(1337);
    private Resources res;
    private FrameCounter frameCounter;
    private float deltaTime = 0;
    private Target target;
    private int width = 1280;
    private int height = 720;

    public Target Target => target;

    public TestSample(App ctx,Batcher batcher)
    {
        this.ctx = ctx;
        world = World.Create();
        Engine.Asset.Assets.Load(ctx.GraphicsDevice);
        
        frameCounter = new FrameCounter();
        target = new Target(ctx.GraphicsDevice,width,height);
        res = new Resources(
            target,
            Engine.Asset.Assets.Font,
            Engine.Asset.Assets.Atlas,
            batcher,
            new Vector2(width,height));
        modules = new Group<float>("TestGroup",
            new StateSystem(world,ctx,res,frameCounter),
            new BuildingCatchSystem(world,res),
            new DestroyNotActiveEntitySystem(world),
            //new RandomPositionSystem(world,rng),
            new FindLineSystem(world,rng),
            
            new CameraSystem(world,ctx,target),
            new TransformSystem(world),
            
            new RenderSystem(world,res.batcher,target)
            /*new PrintEntitiesSystem(world)*/);
    }
    

    public void Start()
    {
        Profiler.AppInfo("Hello AppInfo!");
        modules.Initialize();
        
    }

    public void Destroy()
    {
        modules.Dispose();
    }

    public void Update()
    {
#if DEBUG
        using var zone = Profiler.BeginZone("Update");
#endif
        deltaTime = ctx.Time.Delta;
        modules.Update(in deltaTime);
        
    }

    public void Render()
    {
#if DEBUG
        using (Profiler.BeginZone("Render"))
        {
#endif
            
            target.Clear(Color.White);
            modules.AfterUpdate(in deltaTime);
            res.batcher.Render(target);
            res.batcher.Clear();
#if DEBUG
        }
        Profiler.EmitFrameMark();
#endif
        
        
    }

    
}


