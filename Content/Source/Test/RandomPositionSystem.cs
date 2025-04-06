using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Source.Camera;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;
using Vector2 = System.Numerics.Vector2;

namespace Content.Test;

public partial class RandomPositionSystem:BaseSystem<World,float>
{
    private World world;
    private Rng rng;
    private float minRad;
    private float maxRad;
    public RandomPositionSystem(World world,Rng rng) : base(world)
    {
        this.world = world;
        this.rng = rng;
        minRad = -5 * Calc.DegToRad;
        maxRad = 5 * Calc.DegToRad;
    }
    

    [Query]
    [All<Transform>,None<HasParent,Camera2D>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRandomPosition(ref Transform transform)
    {
        var newPos = transform.localPosition + new Vector2(rng.Float(-1f,1f),rng.Float(-1f,1f));
        var newRotation = Math.Clamp(transform.rad + rng.Float(-5f,5f) * Calc.DegToRad,minRad,maxRad); ;
        
        transform.SetLocalPosition(newPos);
        transform.SetLocalRotation(newRotation);
    }
}