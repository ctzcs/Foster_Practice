using System.Numerics;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public partial class RandomPosition:BaseSystem<World,float>
{
    private World world;
    private Rng rng; 
    public RandomPosition(World world,Rng rng) : base(world)
    {
        this.world = world;
        this.rng = rng;
    }

    [Query]
    [All<Transform>,None<ChildOf>]
    public void SetRandomPosition(ref Transform transform)
    {
        var newPos = transform.localPosition + new Vector2(rng.Float(0,1),rng.Float(0,1));
        Transform.SetLocalPosition(ref transform,newPos);
    }
}