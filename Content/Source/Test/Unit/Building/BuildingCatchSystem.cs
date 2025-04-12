using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Resources;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Asset;
using Engine.Camera;
using Engine.Transform;
using Color = Foster.Framework.Color;

namespace Content.Test;

public partial class BuildingCatchSystem:BaseSystem<World,float>
{
    private World world;
    public BuildingCatchSystem(World world) : base(world)
    {
        this.world = world;
    }

    [Query]
    [All<Worker,Transform>,None<HasParent>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Catch(in Entity entity,ref Transform transform)
    {
        if (Vector2.DistanceSquared(transform.position,Vector2.Zero) < 9)
        {
            if (transform.children.Count < 0)
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
        
        
        if (Vector2.DistanceSquared(transform.position,new Vector2(990,540)) < 9)
        {
            checkEntities.Clear();
            checkEntities.Add(entity);
            while (checkEntities.Count > 0)
            {
                var root = checkEntities[0];
                var children = root.Get<Transform>().children;
                if ( children.Count > 0)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].SetParent(Entity.Null);
                        children[i].Add<NoActive>();
                        checkEntities.Add(children[i]);
                    }
                }
                checkEntities.RemoveAt(0);
            }
        }
        
        
    }
    
    List<Entity> checkEntities = new List<Entity>();
    
}