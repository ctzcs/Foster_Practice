using System.Runtime.CompilerServices;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Asset;
using Engine.Performance;
using Engine.Transform;
using Foster.Framework;
using Color = Foster.Framework.Color;
using Transform = Engine.Transform.Transform;
using Vector2 = System.Numerics.Vector2;

namespace Content.Test;

public partial class BuildingCatchSystem:BaseSystem<World,float>
{
    private World world;
    private Resources resources;
    public BuildingCatchSystem(World world,Resources res) : base(world)
    {
        this.world = world;
        this.resources = res;
    }


    public override void Update(in float t)
    {
#if DEBUG
        using var zone = Profiler.BeginZone(nameof(BuildingCatchSystem));
#endif
        CatchQuery(world);
        
    }

    [Query]
    [All<Worker,Transform>/*None<HasParent>*/]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Catch(in Entity entity,ref Transform transform)
    {
        if (transform.HasParent)
        {
            return;
        }
        if (Vector2.DistanceSquared(transform.position,Vector2.Zero) < 64)
        {
            if (transform.ChildrenCount <= 0)
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
        
        
        else if (Vector2.DistanceSquared(transform.position,new Vector2(0,resources.logicSize.Y/2)) < 64)
        {
            checkEntities.Clear();
            GetAllChild(checkEntities,entity);
            for (int i = 0; i < checkEntities.Count; i++)
            {
                if (checkEntities[i].IsAlive())
                {
                    // 调试输出
                    ref var childTransform = ref checkEntities[i].Get<Transform>();
                    if (childTransform.Parent != Entity.Null) checkEntities[i].SetParent(Entity.Null);
                    if(!checkEntities[i].Has<NoActive>()) checkEntities[i].Add(new NoActive());
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
        
        var children = root.Get<Transform>().Children;
        if (children?.Count <= 0) return;
        for (int i = 0; i < children?.Count; i++)
        {
            child.Add(children[i]);
            GetAllChild(child,children[i]);
        }
    }
    
}