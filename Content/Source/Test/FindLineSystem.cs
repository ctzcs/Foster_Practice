using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public partial class FindLineSystem:BaseSystem<World,float>
{
    private World world;
    private Rng rng;
    private float speed = 5;
    public FindLineSystem(World world,Rng rng) : base(world)
    {
        this.world = world;
        this.rng = rng;
    }


    public override void Update(in float t)
    {
        _lineEntities.Clear();
        AllLineEntitiesQuery(world);
        GetTransformQuery(world);
    }

    private List<Entity> _lineEntities = new List<Entity>();

    [Query]
    [All<LineRenderer>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AllLineEntities(in Entity entity)
    {
        _lineEntities.Add(entity);
    }

    [Query]
    [All<Transform, SpriteRenderer>, None<HasParent>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetTransform(in Entity entity,ref Transform transform)
    {
        if (_lineEntities.Count == 0)
        {
            return;
        }
        if (entity.Has<FollowLine>())
        {
            ref var followLine = ref entity.Get<FollowLine>();
            if (followLine.line.IsAlive())
            {
                ref var lineRenderer = ref followLine.line.Get<LineRenderer>();
                if (followLine.nextIndex < lineRenderer.line.Count )
                {
                   var pos = lineRenderer.line[followLine.nextIndex];
                   var dir = (pos - transform.localPosition).Normalized();
                   if (Vector2.DistanceSquared(pos,transform.localPosition) > 2 )
                   {
                       Transform.SetLocalPosition(ref transform, transform.localPosition + dir*speed);
                   }
                   else
                   {
                       followLine.nextIndex++;
                       if (followLine.nextIndex > lineRenderer.line.Count - 1)
                       {
                           var index = rng.Int(0, _lineEntities.Count);
                           followLine.line = _lineEntities[index];
                           followLine.nextIndex = 0;
                       }
                   }
                }
            }
        }
        else
        {
            var index =  rng.Int(0, _lineEntities.Count);
            entity.Add(new FollowLine()
            {
                 line = _lineEntities[index],
                 nextIndex = 0
            });
        }
    }
}