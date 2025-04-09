using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Camera;
using Engine.Source.Other;
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
       public int layer;
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
        
        BeforeEntityRenderQuery(world);
        //先画线
        LineRenderQuery(world);
        //再画Sprite
        BuildSpriteRenderListQuery(world);
        HandleSpriteRenderList();
#if DEBUG
        RenderDebugRectQuery(world);
#endif
        
        AfterEntityRender();
    }

    [Query]
    [All<Camera2D,Transform.Transform>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BeforeEntityRender(in Transform.Transform transform,in Camera2D camera)
    {
        //TODO 推入相机矩阵 相机中心坐标 + 抖动
        // 让所有的非UI元素都会向相机相反方向移动，
        // 放缩的时候都相对原点了
        // 如果是正常的归一化，应该相对相机原点的地方，是坐标原点，所以缺一个NDC和投影坐标系
        //batcher.PushMatrix(-transform.localPosition + camera.shake);
        
        // 渲染系统应用矩阵时：
        

        /*var v1 = Foster.Framework.Transform.CreateMatrix(camera.rect.Center,
            -transform.position, 
            transform.scale / camera.scaleRate,
            -transform.rad);
        
        batcher.PushMatrix( 
            v1
        );*/
        
        batcher.PushMatrix(
            camera.rect.Center,
            transform.scale / camera.scaleRate,
            -transform.position,
            -transform.rad
        );
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AfterEntityRender()
    {
        batcher.PopMatrix();
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
                batcher.Render(window);
                batcher.Clear();
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