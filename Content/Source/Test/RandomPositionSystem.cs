using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;

using Engine.Render;
using Engine.Transform;
using Foster.Framework;
using Transform = Engine.Transform.Transform;
using Vector2 = System.Numerics.Vector2;

namespace Content.Test;

public partial class RandomPositionSystem:BaseSystem<World,float>
{
    private World world;
    private Rng rng;
    private float minRad;
    private float maxRad;
    private float deltaTime;
    public RandomPositionSystem(World world,Rng rng) : base(world)
    {
        this.world = world;
        this.rng = rng;
        minRad = -5 * Calc.DegToRad;
        maxRad = 5 * Calc.DegToRad;
    }


    public override void Update(in float t)
    {
        deltaTime = t;
        SetRandomPositionQuery(world);
    }
    

    [Query]
    [All<Transform,SpriteRenderer>,None<HasParent>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRandomPosition(ref Transform transform)
    {
        //var newPos = transform.localPosition + new Vector2(rng.Float(-1f,1f),rng.Float(-1f,1f));
        //var newRotation = Math.Clamp(transform.rad + rng.Float(-1f,1f) * Calc.DegToRad * deltaTime,minRad,maxRad); ;
        
        //transform.SetLocalPosition(newPos);
        //transform.SetLocalRotation(newRotation);
    }
}