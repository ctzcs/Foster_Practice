﻿using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Camera;
using Engine.Other;
using Engine.Performance;
using Foster.Framework;

namespace Engine.Render;

public partial class RenderSystem:BaseSystem<World,float>
{
    private const int BatchRenderCount = 32768;
    private World world;
    private Batcher batcher;
    private Target renderTarget;
    private int renderCount = 0;
    private readonly List<OrderRecord> entities = new(150000);
    private Matrix3x2 transformMatrix;
    public struct OrderRecord
    {
       public Entity entity;
       public int layer;
       public int order;
    }
    
    public RenderSystem(World world,Batcher batcher,Target renderTarget) : base(world)
    {
        this.world = world;
        this.batcher = batcher;
        this.renderTarget = renderTarget;
    }

    
    public override void Update(in float t){}
    
    public override void AfterUpdate(in float t)
    {

/*#if DEBUG
        using var zone = Profiler.BeginZone(nameof(RenderSystem));
#endif*/

        
        entities.Clear();

        BeforeEntityRenderQuery(world);
        
        //先画线
        LineRenderQuery(world);
        //再画Sprite

#if DEBUG
        using (Profiler.BeginZone("Sort"))
#endif
        {
            BuildSpriteRenderListQuery(world);
        }
        
#if DEBUG
        using (Profiler.BeginZone("DrawSprite"))
#endif
        {
            HandleSpriteRenderList();
        }
        
#if DEBUG
        using (Profiler.BeginZone("CheckBox"))
        {
            RenderDebugRectQuery(world);
        }
#endif
    }

    [Query]
    [All<Camera2D,Transform.Transform>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BeforeEntityRender(in Transform.Transform transform,in Camera2D camera)
    {
        //推入相机矩阵 相机中心坐标 + 抖动
        // 让所有的非UI元素都会向相机相反方向移动，
        // 放缩的时候都相对原点了
        // 如果是正常的归一化，应该相对相机原点的地方，是坐标原点，所以缺一个NDC和投影坐标系
        
        // 渲染系统应用矩阵时：
        
        transformMatrix = Foster.Framework.Transform.CreateMatrix(camera.rect.Center,
            -transform.position, 
            transform.scale / camera.scaleRate,
            -transform.rad);
        
        batcher.PushMatrix(transformMatrix);
        
    }
    
    
    /// <summary>
    /// 线渲染
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="transform"></param>
    /// <param name="sortingOrder"></param>
    [Query]
    [All<Transform.Transform,LineRenderer,SortingOrder>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LineRender(in Entity entity,in Transform.Transform transform, in SortingOrder sortingOrder)
    {
        ref var lineRenderer = ref entity.Get<LineRenderer>();
        lineRenderer.Draw(batcher,in transform);
    }
    
    /// <summary>
    /// 精灵渲染
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="sortingOrder"></param>
    [Query]
    [All<Transform.Transform,SpriteRenderer,SortingOrder>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BuildSpriteRenderList(in Entity entity,in SortingOrder sortingOrder)
    {
        if (!entity.IsAlive())
        {
            return;
        }
        entities.Add(new OrderRecord()
        {
            entity = entity,
            layer = sortingOrder.layer,
            order = sortingOrder.depth
        });
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void HandleSpriteRenderList()
    {
        entities.Sort((a, b) =>
        {
            if (a.layer == b.layer)
            {
                return a.order - b.order;
            }
            return a.layer - b.layer;
        });
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
                batcher.Render(renderTarget);
                batcher.PopMatrix();
                batcher.Clear();
                batcher.PushMatrix(transformMatrix);
            }
        }
        /*batcher.Render(window);
        batcher.Clear();*/
    }


    [Query]
    [All<Transform.Transform,CheckBox>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RenderDebugRect(in CheckBox box)
    {
        batcher.QuadLine(box.rect.TopLeft,box.rect.TopRight,box.rect.BottomRight,box.rect.BottomLeft,0.5f,Color.Red);
    }
}