using Arch.Core;
using Arch.System;

namespace Engine.Test;

public class TestSystem : BaseSystem<World, float>
{
    public TestSystem(World world) : base(world)
    {
    }
}