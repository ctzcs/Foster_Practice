using System.Numerics;
using Content.Test;
using Engine;
using Foster.Framework;

namespace Content;

public class GameApp : App
{
    IContent content;
    Batcher batcher;
    public GameApp(in AppConfig config) : base(in config)
    {
        GraphicsDevice.VSync = true;
        Window.Resizable = false;
        UpdateMode = UpdateMode.FixedStep(60);
        //lifetime = new FrogSample(this);
        batcher = new Batcher(GraphicsDevice);
        content = new TestSample(this);
    }

    protected override void Startup()
    {
        content.Start();
    }

    protected override void Shutdown()
    {
        
        content.Destroy();
        
    }

    protected override void Update()
    {
        
        content.Update();
    }
    
    protected override void Render()
    {
        content.Render();
        //batcher.Render(Window);
        // draw screen to window
        {
            //比如Mac上的size就是实际大小的数倍
            var size = Window.BoundsInPixels().Size;
            var center = size/2;
            var screenTarget = content.Target;
            var scale = Calc.Min(
                size.X / (float)screenTarget.Width, 
                size.Y / (float)screenTarget.Height);
            //Log.Info( $"{size}__{scale}__{screenTarget.Bounds}");
            batcher.PushSampler(new(TextureFilter.Nearest, TextureWrap.Clamp, TextureWrap.Clamp));
            batcher.Image(screenTarget, center, screenTarget.Bounds.Size / 2, Vector2.One * scale, 0, Color.White);
            batcher.PopSampler();
            batcher.Render(Window);
            batcher.Clear();
        }
    }

    
}


