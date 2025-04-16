using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Performance;

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
#if DEBUG
        using var zone = Profiler.BeginZone(nameof(DestroyNotActiveEntitySystem));
#endif
        DestroyQuery(world);
    }

    [Query]
    [All<NoActive,Unit>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Destroy(in Entity entity)
    {
        if (entity.IsAlive())
        {
            world.Destroy(entity);
        }
    }
}