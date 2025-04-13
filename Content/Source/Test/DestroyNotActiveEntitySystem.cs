using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;

namespace Content.Test;


public partial class DestroyNotActiveEntitySystem:BaseSystem<World,float>
{
    private World world;
    public DestroyNotActiveEntitySystem(World world) : base(world)
    {
        this.world = world;
    }

    public override void Update(in float deltaTime)
    {
        DestroyQuery(world);
    }

    [Query]
    [All<NoActive,Unit>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Destroy(in Entity entity)
    {
        world.Destroy(entity);
    }
}