
using Arch.AOT.SourceGenerator;

namespace Engine.Transform;

/// <summary>
/// 用于找到根节点
/// 如果挂上了这个节点说明有Parent
/// </summary>
[Component]
public struct HasParent
{
}