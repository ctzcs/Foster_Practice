
using System.IO;
using System.Numerics;
using Arch.Core;
using Arch.System;
using Engine;
using Engine.Camera;
using Engine.Performance;
using Engine.Render;
using Engine.Test;
using Engine.Transform;
using Engine.UI;
using Engine.UI.Widgets;
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
    private UiRoot uiRoot;
    public Target Target => target;

    public TestSample(App ctx,Batcher batcher)
    {
        this.ctx = ctx;
        world = World.Create();
        Engine.Asset.Assets.Load(ctx.GraphicsDevice);
        var font = new SpriteFont(ctx.GraphicsDevice, 
            Path.Join(Engine.Asset.Assets.AssetsPath, "Fonts", "SmileySans-Oblique.ttf"), 
            32);
        font.LineGap = 32;
        
        
        Engine.Asset.Assets.SetFont(font);
        
        frameCounter = new FrameCounter();
        target = new Target(ctx.GraphicsDevice,width,height);
        res = new Resources(
            target,
            font,
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
        
        
        uiRoot = new UiRoot(ctx.Input,ctx.Window,res.logicSize);
        uiRoot.Root.AddChild(new Button(true,true,true,new Rect(0,0,400,400),null));
    }
    

    public void Start()
    {
#if DEBUG
        Profiler.AppInfo("Test Sample!");
#endif
        
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
        uiRoot.Update();
    }

    public void Render()
    {
#if DEBUG
        using (Profiler.BeginZone("Render"))
#endif
        {
            target.Clear(Color.White);
            modules.AfterUpdate(in deltaTime);
            res.batcher.Render(target);
            res.batcher.Clear();
            uiRoot.AfterUpdate(res.batcher);
            res.batcher.Render(target);
            res.batcher.Clear();
        }
        
#if DEBUG
        Profiler.EmitFrameMark();
#endif
        
        
    }

    
}


