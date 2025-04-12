
using Arch.AOT.SourceGenerator;
using Engine.Core.Structure;

namespace Engine.Physics.SpatialHash;
[Component]
public struct SpatialHash
{
    public Vector2Int index;
    public Vector2Int chunkIndex;
    public Vector2Int gridIndex;
}