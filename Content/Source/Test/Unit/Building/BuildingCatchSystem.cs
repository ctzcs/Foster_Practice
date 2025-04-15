
using System.Runtime.CompilerServices;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Asset;
using Engine.Camera;
using Engine.Transform;
using Foster.Framework;
using Color = Foster.Framework.Color;
using Transform = Engine.Transform.Transform;
using Vector2 = System.Numerics.Vector2;

namespace Content.Test;

public partial class BuildingCatchSystem:BaseSystem<World,float>
{
    private World world;
    private CommandBuffer commandBuffer;
    public BuildingCatchSystem(World world) : base(world)
    {
        this.world = world;
        this.commandBuffer = new CommandBuffer();
    }


    public override void Update(in float t)
    {
        CatchQuery(world);
        commandBuffer.Playback(world,true);
        
    }

    [Query]
    [All<Worker,Transform>,None<HasParent>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Catch(in Entity entity,ref Transform transform)
    {
        if (Vector2.DistanceSquared(transform.position,Vector2.Zero) < 250)
        {
            if (transform.children.Count <= 0)
            {
                var child = TestExt.CreateFrogCarrier(
                    world,
                    new Vector2(0,-10),
                    0,
                    Vector2.One,
                    Assets.GetSubtexture("frog/0"),
                    Color.Blue,
                    4);
                child.SetParent(entity);
            }
        }
        
        
        if (Vector2.DistanceSquared(transform.position,new Vector2(640,360)) < 250)
        {
            checkEntities.Clear();
            
            GetAllChild(checkEntities,entity);
            
            for (int i = 0; i < checkEntities.Count; i++)
            {
                if (checkEntities[i].IsAlive())
                {
                    // 调试输出
                    Log.Info($"Child Entity: {checkEntities[i]}");
                    ref var childTransform = ref checkEntities[i].Get<Transform>();
                    if (childTransform.parent != Entity.Null)
                    {
                        checkEntities[i].SetParent(Entity.Null);
                    }
                    //checkEntities[i].Add(new NoActive());
                    commandBuffer.Destroy(checkEntities[i]);
                }
                    
            }
            
            
            
        }
        
        
    }
    
    List<Entity> checkEntities = [];

    void GetAllChild(List<Entity> child,in Entity root)
    {
        if (!root.IsAlive() || !root.Has<Transform>())
        {
            return;
        }
        
        var children = root.Get<Transform>().children;
        if (children?.Count <= 0) return;
        for (int i = 0; i < children?.Count; i++)
        {
            child.Add(children[i]);
            //GetAllChild(child,children[i]);
        }
    }
    
}