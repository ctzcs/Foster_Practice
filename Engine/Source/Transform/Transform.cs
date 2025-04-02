using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;

namespace Engine.Source.Transform;

public struct Transform
{
    public Entity parent;
    
    public List<Entity> children;
    /// <summary>
    /// Local用来表示相对父节点的偏移
    /// </summary>
    public Vector2 localPosition;

    /// WorldPos用来渲染
    public Vector2 worldPosition;
    
    public float rotation;
    
    public Vector2 scale;
    
    public bool isDirty;
    
    
    
    /// <summary>
    /// 设置本地坐标后，local变了，也是相对父节点变化，延迟计算世界坐标
    /// </summary>
    /// <param name="transform"></param>
    internal static void CalculateWorldPosition(ref Transform transform)
    {
        if (transform.isDirty)
        {
            if (transform.parent != Entity.Null)
            {
                ref var parentTransform = ref transform.parent.Get<Transform>();
                if (parentTransform.isDirty)
                {
                    CalculateWorldPosition(ref parentTransform);
                }
                transform.worldPosition = parentTransform.worldPosition + transform.localPosition;
            }
            else
            {
                transform.worldPosition = transform.localPosition;
            }
            transform.isDirty = false;
        }
    }

    /// <summary>
    /// 直接设置世界坐标的时候，世界坐标已经算好了，其实是本地坐标相对父节点变化,应该立即调用
    /// </summary>
    /// <param name="transform"></param>
    internal static void CalculateLocalPosition(ref Transform transform)
    {
        if (transform.isDirty)
        {
            if (transform.parent != Entity.Null)
            {
                ref var parentTransform = ref transform.parent.Get<Transform>();
                CalculateWorldPosition(ref parentTransform);
                transform.localPosition = transform.worldPosition - parentTransform.worldPosition;
            }
            else
            {
                transform.localPosition = transform.worldPosition;
            }
            transform.isDirty = false;
        }
    }
    
    //更新所有根节点坐标
    //更新所有叶子节点坐标，叶子节点会递归更新所有父节点坐标
    
    //父节点的本地或者世界坐标变换的时候，所有子节点的坐标标记为脏


    public static void SetWorldPosition(ref Transform transform, Vector2 worldPosition)
    {
        transform.worldPosition = worldPosition;
        CalculateWorldPosition(ref transform);
        DirtyMake(ref transform,false);
    }

    public static void SetLocalPosition(ref Transform transform, Vector2 localPosition)
    {
        transform.localPosition = localPosition;
        DirtyMake(ref transform,true);
    }
    
    static void DirtyMake(ref Transform transform,bool selfDirty)
    {
        if (selfDirty)
        {
            transform.isDirty = true;
        }
        var children = transform.children;
        for (int i = 0; i < children.Count; i++)
        {
            children[i].Get<Transform>().isDirty = true;
        }
    }
}

