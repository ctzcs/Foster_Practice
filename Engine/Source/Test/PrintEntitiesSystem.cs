using Arch.Core;
using Arch.System;
using Engine.Utility;
using Foster.Framework;

namespace Engine.Test;

public partial class PrintEntitiesSystem : BaseSystem<World, float>
{
    public PrintEntitiesSystem(World world) : base(world)
    {
    }

    [Query]
    [All]
    void PrintEntities(in Entity entity)
    {
        Log.Info($"{entity.Id}:{EcsUtils.PrintEntity(entity)}");
    }
}