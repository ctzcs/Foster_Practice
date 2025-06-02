using Foster.Framework;
using System.Numerics;
using System.Reflection;
using Content;
using Content.Test;
using Engine;
using ImGuiNET;

namespace Editor;
class Editor : App
{
    private readonly Texture image;
    private readonly Renderer imRenderer;
    private IContent content;
    public Editor() : base(new AppConfig()
    {
        ApplicationName = "ImGuiExample",
        WindowTitle = "Dear ImGui x Foster",
        Width = 1280,
        Height = 720,
        Resizable = true
    })
    {
        image = new Texture(GraphicsDevice, new Image("button.png"));
        imRenderer = new(this);
        //TODO 这里要改成具体的
        content = new TestSample(this);
    }
    

    protected override void Startup()
    {
        content.Start();
        
    }

    protected override void Shutdown()
    {
        content.Destroy();
        imRenderer.Dispose();
    }

    protected override void Update()
    {
        //游戏内容更新
        content.Update();
        
        imRenderer.BeginLayout();
        EditorSettingStart();
        EditorSettingUpdate();
        UpdateMainMenuBar();
        ImGui.BeginGroup();
        UpdateInspector();
        UpdateAssets();
        UpdateViewPort();
        UpdateHierarchy();
        UpdateSystem();
        ImGui.EndGroup();
        imRenderer.EndLayout();
    }

    protected override void Render()
    {
        Window.Clear(Color.Black);
        
        /*batcher.Render(Window);
        batcher.Clear();*/
        imRenderer.Render();
        
        
    }

    void EditorSettingStart()
    {
        
    }
    void EditorSettingUpdate()
    {
        ImGui.DockSpaceOverViewport();
        // toggle text input if ImGui wants it
        if (imRenderer.WantsTextInput)
            Window.StartTextInput();
        else
            Window.StopTextInput();
    }

    void UpdateMainMenuBar()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Open"))
                {
                    // TODO: open file
                }
                if (ImGui.MenuItem("Save"))
                {
                    // TODO: save file
                }
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
    }
    private void UpdateInspector()
    {
        if(ImGui.Begin("Inspector"))
        {
            ImGui.Text("ObjectName");
            if (ImGui.TreeNode("Component"))
            {
                ImGui.Text("Property");
                ImGui.TreePop();
            }
        }
        ImGui.End();
    }

    void UpdateHierarchy()
    {
        //ImGui.SetNextWindowSize(new Vector2(400, 300), ImGuiCond.Appearing);
        if (ImGui.Begin("Hierarchy"))
        {
            if (ImGui.CollapsingHeader("[Player]"))
            {
                if (ImGui.TreeNode("Player"))
                {
                    ImGui.TreePop();
                }
            }
            
            
            if (ImGui.CollapsingHeader("[Enemy]"))
            {
                if (ImGui.TreeNode("MonsterA"))
                {
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("MonsterB"))
                {
                    ImGui.TreePop();
                }
            }
            
        }
        ImGui.End();
    }


    void UpdateSystem()
    {
        if (ImGui.Begin("System"))
        {
            if (ImGui.CollapsingHeader("[SystemGroup]"))
            {
                if (ImGui.TreeNode("SystemA"))
                {
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("SystemB"))
                {
                    ImGui.TreePop();
                }
            }
            //TODO System Content
        }
        ImGui.End();
    }
    
    void UpdateViewPort()
    {
        //ImGui.SetNextWindowSize(new Vector2(320, 180), ImGuiCond.Appearing);
        if (ImGui.Begin("ViewPort", ImGuiWindowFlags.MenuBar))
        {
            var size = ImGui.GetContentRegionAvail();
            var scaleRate = 1f;
            if (ImGui.BeginMenuBar())
            {
                ImGui.SetNextItemWidth(80);
                if (ImGui.SliderFloat("##ScaleSlider", ref scaleRate, 0.5f, 6.0f))
                {
                    
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("ScaleRate");
                    ImGui.EndTooltip();
                }
            }
            ImGui.EndMenuBar();
            
            if (imRenderer.BeginBatch(size, out var batch, out var bounds))
            {
                content.Render();
                var wsize = size;
                var center = wsize/2;
                var screenTarget = content.Target;
                var scale = Calc.Min(
                    wsize.X / (float)screenTarget.Width, 
                    wsize.Y / (float)screenTarget.Height) * scaleRate;
                //Log.Info( $"{size}__{scale}__{screenTarget.Bounds}");
                    
                batch.PushSampler(new(TextureFilter.Nearest, TextureWrap.Clamp, TextureWrap.Clamp));
                batch.Image(screenTarget, center, screenTarget.Bounds.Size / 2, Vector2.One * scale, 0, Color.White);
                batch.PopSampler();
                    
            }
            //因为已经清空了所以不需要endbatch
            imRenderer.EndBatch();
                
                
        }
        ImGui.End();
    }

    void UpdateAssets()
    {
        // 这里放你的子窗口、内容等
        if (ImGui.Begin("Assets"))
        {
                
        }
        ImGui.End();
    }
}