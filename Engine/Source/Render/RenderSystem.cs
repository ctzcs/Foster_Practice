using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Foster.Framework;

namespace Engine.Source.Render;

public partial class RenderSystem:BaseSystem<World,float>
{
    private World world;
    private Batcher _batcher;
    private Window window;
    private const int BatchRenderCount = 32768;
    private int wholeCount = 0;
    int renderCount = 0;
    public RenderSystem(World world,Batcher batcher,Window window) : base(world)
    {
        this.world = world;
        _batcher = batcher;
        this.window = window;
    }

    public override void AfterUpdate(in float t)
    {
        renderCount = 0;
        RenderQuery(world);
        
    }


    [Query]
    [All<Transform.Transform, SpriteRenderer>]
    public void Render(in Transform.Transform transform,in SpriteRenderer spriteRenderer)
    {
        renderCount++;
        _batcher.Image(spriteRenderer.texture, in transform.worldPosition,spriteRenderer.color);
        if (renderCount > BatchRenderCount )
        {
            renderCount = 0;
            _batcher.Render(window);
            _batcher.Clear();
        }
    }
}