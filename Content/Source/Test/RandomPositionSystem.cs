
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;
using Vector2 = System.Numerics.Vector2;

namespace Content.Test;

public partial class RandomPositionSystem:BaseSystem<World,float>
{
    private World world;
    private Rng rng; 
    public RandomPositionSystem(World world,Rng rng) : base(world)
    {
        this.world = world;
        this.rng = rng;
    }
    

    [Query]
    [All<Transform>,None<HasParent,LineRenderer>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRandomPosition(ref Transform transform)
    {
        var newPos = transform.localPosition + new Vector2(rng.Float(-1f,1f),rng.Float(-1f,1f));
        Transform.SetLocalPosition(ref transform,newPos);
    }
}