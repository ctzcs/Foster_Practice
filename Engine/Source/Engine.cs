using Arch.Core;
using Arch.System;
using Engine.Source.Test;
using Engine.Source.Transform;

namespace Engine.Source;

public class Engine
{
    public static void Main()
    {
        var world = World.Create();
        var group2 = new Group<float>(
            ConstStr.TEST,
            new TestSystem(world));
        var group = new Group<float>(
            ConstStr.ENGINE,
            new TransformSystem(world),
            group2);

        var deltaTime = 0.2f;
        group.Initialize();
        group.BeforeUpdate(in deltaTime);
        group.Update(in deltaTime);
        group.AfterUpdate(in deltaTime);
        group.Dispose();
    }
}