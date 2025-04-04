using Engine.Source;
using Foster.Framework;

namespace Content;

public class GameApp : App
{
    ILifetime lifetime;
    public GameApp(in AppConfig config) : base(in config)
    {
        GraphicsDevice.VSync = true;
        Window.Resizable = true;
        UpdateMode = UpdateMode.FixedStep(60);
        //lifetime = new FrogSample(this);
        lifetime = new TestSample(this);
    }

    protected override void Startup()
    {
        
        lifetime.Start();
    }

    protected override void Shutdown()
    {
        
        lifetime.Destroy();
        
    }

    protected override void Update()
    {
        
        lifetime.Update();
    }
    
    protected override void Render()
    {
        
        lifetime.Render();
    }

    
}


