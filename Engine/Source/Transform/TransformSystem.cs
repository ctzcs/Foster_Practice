using Arch.Core;
using Arch.System;

namespace Engine.Source.Transform;

public class TransformSystem:BaseSystem<World,float>
{
    public TransformSystem(World world) : base(world)
    {
    }
}