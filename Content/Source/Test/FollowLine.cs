using Arch.AOT.SourceGenerator;
using Arch.Core;

namespace Content.Test;

[Component]
public struct FollowLine
{
    public Entity line;
    public int nextIndex;
}