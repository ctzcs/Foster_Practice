using Arch.AOT.SourceGenerator;

namespace Engine.Render;

[Component]
public struct SortingOrder
{
    public int layer;
    public int depth;
}