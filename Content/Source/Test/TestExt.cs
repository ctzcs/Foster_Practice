using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Engine.Source.Render;
using Foster.Framework;
using Transform = Engine.Source.Transform.Transform;

namespace Content.Test;

public static class TestExt
{
    public static Entity CreateSimpleFrog(World world,Vector2 position, Vector2 size,Texture tex, Color color)
    {
        var ent = world.Create();
        ent.Add(new Transform()
        {
            parent = Entity.Null,
            children = new(),
            localPosition = position,
            scale = size,
            isDirty = true
        });
        ent.Add(new SpriteRenderer()
        {
            texture = tex,
            color = color,
        });
        return ent;
    }
}