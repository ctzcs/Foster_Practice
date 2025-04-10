using Arch.Core;
using Arch.System;

namespace Content.Test;

public class BuildingCatchSystem:BaseSystem<World,float>
{
    public BuildingCatchSystem(World world) : base(world)
    {
    }

    //TODO 抓住所有到达建筑的实体，放下东西后，让其去资源建筑获取资源

    
}