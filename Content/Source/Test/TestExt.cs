using System.Numerics;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Engine.Source.Other;
using Engine.Source.Render;
using Engine.Source.Transform;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public static class TestExt
{
    public static Entity CreateSimpleFrog(World world,Vector2 position,float rotation ,Vector2 size,Subtexture tex, Color color,int depth = 0)
    {
        var ent = world.Create(
            new Unit(),
            new Worker(),
            new Transform(Entity.Null, position, rotation, size),
            new CheckBox() { rect = new Rect(position, 16, 16) },
        new SpriteRenderer()
          {
              subtexture = tex,
              color = color,
              origin = new (tex.Width/2f,tex.Height/2f),
          },
        new SortingOrder()
        {
            layer = ELayer.Frog.GetId(),
            depth = depth
        });
        
        return ent;
    }
    
    public static Entity CreateBuilding(World world, Vector2 position, float rotation, Vector2 size, Subtexture tex,
        Color color, int depth = 0)
    {
        var ent = world.Create(
            new Unit(),
            new Building(),
            new Transform(Entity.Null, position, rotation, size),
            new CheckBox() { rect = new Rect(position, 32, 32) },
            new SpriteRenderer()
            {
                subtexture = tex,
                color = color,
                origin = new (tex.Width/2f,tex.Height/2f),
            },
            new SortingOrder()
            {
                layer = ELayer.Building.GetId(),
                depth = depth
            });
        
        return ent;
    }
    
    public static Entity CreatLine(World world,Vector2 position,float rotation ,Vector2 size,Color color,float linewidth,int depth = 0)
    {
        var ent = world.Create(
            new Unit(),
            new Transform(Entity.Null,position,rotation,size),
            new LineRenderer()
            {
                line = new List<Vector2>(),
                color = color,
                lineWidth = linewidth,
            },
            new SortingOrder()
            {
                layer = ELayer.Line.GetId(),
                depth = depth
            });
        
        return ent;
    }

    public static void CreateFrogCarrier(World world,Vector2 pos,float rotation,Vector2 size,Subtexture texture,Color color,int wholeCount)
    {
        var root = CreateSimpleFrog
            (world, pos,rotation ,size, texture, color);
        var last = root;
        for (int i = 0; i < wholeCount - 1; i++)
        {
            var e = CreateSimpleFrog
                (world, new Vector2(0, -10), 0,Vector2.One,texture,new Color(0,1,0,0.5f), i);
            e.SetParent(last);
            last = e;
        }
       
    }



    public static void DestroyUnit(in Entity entity)
    {
        
    }
    
}