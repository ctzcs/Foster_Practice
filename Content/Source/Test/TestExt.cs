using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public static class TestExt
{
    public static Entity CreateSimpleFrog(World world,Vector2 position, Vector2 size,Texture tex, Color color,int depth = 0)
    {
        var ent = world.Create(
            new Transform()
        {
            parent = Entity.Null,
            children = new(),
            localPosition = position,
            scale = size,
            isDirty = true
        },
        new SpriteRenderer()
          {
              texture = tex,
              color = color,
          },
        new SortingOrder()
        {
            depth = depth
        });
        
        return ent;
    }


    public static Entity CreatLine(World world,Vector2 position, Vector2 size,Color color,float linewidth,int depth = 0)
    {
        var ent = world.Create(new Transform()
            {
                parent = Entity.Null,
                children = new(),
                localPosition = position,
                scale = size,
                isDirty = true
            },
            new LineRenderer()
            {
                line = new List<Vector2>(),
                color = color,
                lineWidth = linewidth,
            },
            new SortingOrder()
            {
                depth = depth
            });
        
        return ent;
    }

    public static void CreateFrogCarrier(World world,Vector2 pos,Texture texture,Color color,int wholeCount)
    {
        var root = CreateSimpleFrog
            (world, pos, Vector2.One, texture, color, 0);
        var last = root;
        for (int i = 0; i < wholeCount; i++)
        {
            var e = CreateSimpleFrog
                (world, new Vector2(0, -10), Vector2.One, texture,new Color(0,1,0,0.5f), i);
            e.SetParent(last);
            last = e;
        }
       
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetParent(this Entity child, Entity parent)
    {
        if (parent == Entity.Null)
        {
            ref var childTransform = ref child.Get<Transform>();
            var preParent = childTransform.parent;
            if (preParent != Entity.Null)
            {
                if (parent.Has<Transform>())
                {
                    var parentTransform = preParent.Get<Transform>();
                    if (parentTransform.children.Count <= 0)
                    {
                        parent.Remove<HasChild>();
                    }
                }
                
            }
            child.Remove<HasParent>();
            childTransform.parent = Entity.Null;
            return;
        }
        if(parent.Has<Transform>()) 
            parent.Get<Transform>().children.Add(child);
        if(child.Has<Transform>())
            child.Get<Transform>().parent = parent;
        if(!child.Has<HasParent>()) child.Add<HasParent>();
        if(!parent.Has<HasChild>())parent.Add<HasChild>();
    }
    
    
    
    
}