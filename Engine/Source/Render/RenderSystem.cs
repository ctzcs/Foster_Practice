using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Foster.Framework;

namespace Engine.Source.Render;

public partial class RenderSystem:BaseSystem<World,float>
{
    private const int BatchRenderCount = 32768;
    private World world;
    private Batcher batcher;
    private Window window;
    private int renderCount = 0;
    private readonly List<OrderRecord> entities = new(150000);
    
    public struct OrderRecord
    {
       public Entity entity;
       public int order;
    }
    
    public RenderSystem(World world,Batcher batcher,Window window) : base(world)
    {
        this.world = world;
        this.batcher = batcher;
        this.window = window;
    }

    public override void AfterUpdate(in float t)
    {
        entities.Clear();
        LineRenderQuery(world);//先画线
        //再画Sprite
        BuildSpriteRenderListQuery(world);
        HandleSpriteRenderList();
    }
    
    
    
    

    [Query]
    [All<Transform.Transform,LineRenderer,SortingOrder>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LineRender(in Entity entity,in Transform.Transform transform, in SortingOrder sortingOrder)
    {
        ref var lineRenderer = ref entity.Get<LineRenderer>();
        lineRenderer.Draw(batcher,in transform);
    }
    
    [Query]
    [All<Transform.Transform,SpriteRenderer,SortingOrder>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BuildSpriteRenderList(in Entity entity,in SortingOrder sortingOrder)
    {
        entities.Add(new OrderRecord()
        {
            entity = entity,
            order = sortingOrder.depth
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void HandleSpriteRenderList()
    {
        entities.Sort((a, b) => a.order - b.order);
        renderCount = 0;
        for (int i = 0; i < entities.Count; i++)
        {
            var entity = entities[i].entity;
            ref var transform = ref entity.Get<Transform.Transform>();
            ref var spriteRenderer = ref entity.Get<SpriteRenderer>();
            spriteRenderer.Draw(batcher,in transform);
            renderCount++;
            if (renderCount > BatchRenderCount )
            {
                renderCount = 0;
                batcher.Render(window);
                batcher.Clear();
            }
        }
        
    }
}