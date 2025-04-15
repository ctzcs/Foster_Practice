using Foster.Framework;
using System.Numerics;
using System.Reflection;
using Content.Test;
using Engine;
using ImGuiNET;

namespace Editor;
class Editor : App
{
    private readonly Texture image;
    private readonly Renderer imRenderer;
    private readonly ILifetime lifetime;
    private Batcher batcher;
    public Editor() : base(new AppConfig()
    {
        ApplicationName = "ImGuiExample",
        WindowTitle = "Dear ImGui x Foster",
        Width = 1280,
        Height = 720
    })
    {
        image = new Texture(GraphicsDevice, new Image("button.png"));
        imRenderer = new(this);
        batcher = new Batcher(GraphicsDevice);
        lifetime = new TestSample(this, batcher);
    }

    protected override void Startup()
    {
        lifetime.Start();
    }

    protected override void Shutdown()
    {
        lifetime.Destroy();
        imRenderer.Dispose();
    }

    protected override void Update()
    {
        imRenderer.BeginLayout();

        // toggle text input if ImGui wants it
        if (imRenderer.WantsTextInput)
            Window.StartTextInput();
        else
            Window.StopTextInput();

        ImGui.SetNextWindowSize(new Vector2(400, 300), ImGuiCond.Appearing);
        if (ImGui.Begin("Hello Foster x Dear ImGui"))
        {
            // show an Image button
            var imageId = imRenderer.GetTextureID(image);
            if (ImGui.ImageButton("Image", imageId, new Vector2(32, 32)))
                ImGui.OpenPopup("Image Button");

            // image buttton popup
            if (ImGui.BeginPopup("Image Button"))
            {
                ImGui.Text("You pressed the Image Button!");
                ImGui.EndPopup();
            }

            // custom sprite batcher inside imgui window
            ImGui.Text("Some Foster Sprite Batching:");
            var size = new Vector2(ImGui.GetContentRegionAvail().X, 200);
            if (imRenderer.BeginBatch(size, out var batch, out var bounds))
            {
                batch.CheckeredPattern(bounds, 16, 16, Color.DarkGray, Color.Gray);
                //batch.Circle(bounds.Center, 32, 16, Color.Red);
                
            }
            lifetime.Update();
            imRenderer.EndBatch();

            ImGui.Text("That weas pretty cool!");	
        }
        
        
        ImGui.End();

        ImGui.ShowDemoWindow();

        imRenderer.EndLayout();
    }

    protected override void Render()
    {
        Window.Clear(Color.Black);
        lifetime.Render();
        batcher.Render(Window);
        batcher.Clear();
        imRenderer.Render();
        
        
    }
}