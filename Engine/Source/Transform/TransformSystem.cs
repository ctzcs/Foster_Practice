using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.System;
using Arch.System.SourceGenerator;
using Engine.Other;
using Engine.Performance;

namespace Engine.Transform;

public partial class TransformSystem : BaseSystem<World, float>
{
    
    public TransformSystem(World world) : base(world)
    {
    }
    
    /*/// <summary>
    /// 所有根节点操作
    /// </summary>
    /// <param name="transform"></param>
    [Query]
    [All<Transform>, None<ChildOf>]
    public void RootTransform(ref Transform transform)
    {
        Transform.CalculateWorldPosition(ref transform);
    }*/


    public override void Update(in float t)
    {
#if DEBUG
        using var zone = Profiler.BeginZone(nameof(TransformSystem));
#endif
            LeafTransformQuery(World);
            UpdateCheckBoxQuery(World);
    }
    

    /// <summary>
    /// 所有的叶子节点操作
    /// </summary>
    /// <param name="transform"></param>
    [Query]
    [All<Transform>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LeafTransform(ref Transform transform)
    {
        if (!transform.HasChildren)
        {
            transform.UpdateTransform();
        }
    }

    [Query]
    [All<Transform,CheckBox>]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateCheckBox(ref Transform transform, ref CheckBox checkBox)
    {
        checkBox.rect.Center = transform.position;
    }
    
    
}