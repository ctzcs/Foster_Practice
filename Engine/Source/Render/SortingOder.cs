using Arch.AOT.SourceGenerator;

namespace Engine.Source.Render;

[Component]
public struct SortingOrder
{
    public int layer;
    public int depth;
}