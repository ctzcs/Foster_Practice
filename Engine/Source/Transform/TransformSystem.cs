using Arch.Core;
using Arch.System;

namespace Engine.Source.Transform;

public class TransformSystem:BaseSystem<World,float>
{
    public TransformSystem(World world) : base(world)
    {
    }

    public override void Update(in float t)
    {
        base.Update(in t);
        
    }
    
    
    //更新所有根节点坐标
    //更新所有叶子节点坐标，叶子节点会递归更新所有父节点坐标
}