﻿using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Foster.Framework;

namespace Engine.Transform;

public static class TransformExt
{
    
    /// <summary>
    /// 设置本地坐标后，local变了，也是相对父节点变化，延迟计算世界坐标
    /// </summary>
    /// <param name="transform"></param>
    internal static void UpdateTransform(ref this Transform transform)
    {
        if (transform.hierarchyDirty == Transform.EDirtyType.Clean) return;
        
        if (transform.Parent != Entity.Null)
        {
            UpdateTransform(ref transform.Parent.Get<Transform>());
        }
        
        if ((transform.hierarchyDirty & Transform.EDirtyType.PositionDirty) != 0)
        {
            transform.translationMatrix = Matrix3x2.CreateTranslation(transform.localPosition);
            //transform.position = parentTransform.position + transform.localPosition;
        }
    
        if ((transform.hierarchyDirty & Transform.EDirtyType.RotationDirty) != 0)
        {
            transform.rotationMatrix = Matrix3x2.CreateRotation(transform.localRad);
            //transform.rotation = parentTransform.rotation + transform.localRotation;
        }
    
    
        if ((transform.hierarchyDirty & Transform.EDirtyType.ScaleDirty) != 0)
        {
            transform.scaleMatrix = Matrix3x2.CreateScale(transform.localScale); 
            //transform.scale = parentTransform.scale * transform.localScale;
        }
        
        //SRT
        //TRS
        transform.localTransform = Matrix3x2.Multiply(transform.scaleMatrix,transform.rotationMatrix);
        transform.localTransform = Matrix3x2.Multiply(transform.localTransform,transform.translationMatrix);

        if (transform.Parent == Entity.Null)
        {
            transform.worldTransform = transform.localTransform;
            transform.position = transform.localPosition;
            transform.rad = transform.localRad;
            transform.scale = transform.localScale;
        }
        else
        {
            ref var parentTransform = ref transform.Parent.Get<Transform>();
            
            transform.worldTransform = Matrix3x2.Multiply(transform.localTransform,parentTransform.worldTransform);

            transform.rad = transform.localRad + parentTransform.rad;
            transform.scale = transform.localScale * parentTransform.scale;
            transform.position = new Vector2(transform.worldTransform.M31, transform.worldTransform.M32); 
            
        }

        transform.hierarchyDirty = Transform.EDirtyType.Clean;
    }

    
    /*/// <summary>
    /// 直接设置世界坐标的时候，世界坐标已经算好了，其实是本地坐标相对父节点变化,应该立即调用
    /// </summary>
    /// <param name="transform"></param>
    internal static void CalculateLocalPosition(ref Transform transform)
    {
        if (!transform.isDirty) return;
        if (transform.parent != Entity.Null)
        {
            ref var parentTransform = ref transform.parent.Get<Transform>();
            UpdateTransform(ref parentTransform);
            transform.localPosition = transform.position - parentTransform.position;
        }
        else
        {
            transform.localPosition = transform.position;
        }
        transform.isDirty = false;
    }*/
    
    
    //更新所有根节点坐标
    //更新所有叶子节点坐标，叶子节点会递归更新所有父节点坐标
    
    //父节点的本地或者世界坐标变换的时候，所有子节点的坐标标记为脏
    
    /*public static ref Transform SetWorldPosition(ref this Transform transform, Vector2 worldPosition)
    {
        transform.position = worldPosition;
        UpdateTransform(ref transform);
        transform.DirtyMake(ref transform,false);
        return ref transform;
    }*/
    
    
    public static ref Transform SetLocalPosition(ref this Transform transform, Vector2 localPosition)
    {
        transform.localPosition = localPosition;
        transform.SetDirty(Transform.EDirtyType.PositionDirty);
        return ref transform;
    }

    public static ref Transform SetLocalRotation(ref this Transform transform, float localRad)
    {
        transform.localRad = localRad;
        transform.SetDirty(Transform.EDirtyType.RotationDirty);
        return ref transform;
    }

    public static ref Transform SetLocalScale(ref this Transform transform, Vector2 scale)
    {
        transform.localScale = scale;
        transform.SetDirty(Transform.EDirtyType.ScaleDirty);
        return ref transform;
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetParent(this Entity child, Entity parent)
    {
        /*CommandBuffer cmb = EcsUtils.Cmb;
        cmb.Dispose();*/
        //打印出parent,preparent,child
        //解除可能会导致localPosition和WorldPosition的问题，
        
        if (parent == Entity.Null)
        {
            ref var childTransform = ref child.Get<Transform>();
            var preParent = childTransform.Parent;
            if ( preParent != Entity.Null)
            {
                if (preParent.Has<Transform>())
                {
                    ref var preParentTransform = ref preParent.Get<Transform>();
                    preParentTransform.RemoveChild(child);
                    /*if (preParentTransform.children.Count <= 0)
                    {
                        //preParent.Remove<HasChild>();
                        cmb.Remove<HasChild>(preParent);
                    }*/
                }
                
            }
            //cmb.Remove<HasParent>(child); //child.Remove<HasParent>();
            childTransform.Parent = Entity.Null;
            
            childTransform.SetLocalPosition(childTransform.position);
            
            //cmb.Playback(World.Worlds.DangerousGetReferenceAt(child.WorldId),true);
            return;
        }
        if(parent.Has<Transform>()) parent.Get<Transform>().AddChild(child);
        if(child.Has<Transform>()) child.Get<Transform>().Parent = parent;
        //if(!child.Has<HasParent>()) cmb.Add<HasParent>(child); //child.Add<HasParent>();
        //if(!parent.Has<HasChild>()) cmb.Add<HasChild>(parent); //parent.Add<HasChild>();
        //cmb.Playback(World.Worlds.DangerousGetReferenceAt(child.WorldId),true);
        
    }
}