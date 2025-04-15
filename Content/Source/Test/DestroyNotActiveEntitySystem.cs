using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
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
        Console.WriteLine("DestroyNotActiveEntitySystem updating...");
        DestroyQuery(world);
    }

    [Query]
    [All<NoActive,Unit>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Destroy(in Entity entity)
    {
        Console.WriteLine($"Trying to destroy entity: {entity}");
        if (entity.IsAlive())
        {
            Console.WriteLine($"Entity {entity} is alive, destroying...");
            world.Destroy(entity);
        }
        else
        {
            Console.WriteLine($"Entity {entity} is not alive");
        }
        
    }
}