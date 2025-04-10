using System.Numerics;

namespace Engine.Physics.SpatialHash
{
    public interface IEntityObj
    {
        Vector2 PrePosition { get; }
        Vector2 Position { get; }
    }
}