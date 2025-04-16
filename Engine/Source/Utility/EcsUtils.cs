using System.Text;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;

namespace Engine.Utility;

public static class EcsUtils
{

    private static readonly StringBuilder Sb = new StringBuilder();
    
    private static CommandBuffer cmb = new CommandBuffer();

    public static CommandBuffer Cmb => cmb;
    public static string PrintEntity(in Entity entity)
    {
        if (!entity.IsAlive()) return string.Empty;
        Sb.Clear();
        var component = entity.GetComponentTypes();
        foreach (var componentType in component)
        {
            Sb.Append($"|{componentType.Type.ToString()}");
        }

        if (entity.Has<Transform.Transform>())
        {
            ref var transform = ref entity.Get<Transform.Transform>();
            Sb.Append($"|Transform:{transform.Parent} {transform.ChildrenCount}");
        }
        return Sb.ToString();
    }
    
    
    
}