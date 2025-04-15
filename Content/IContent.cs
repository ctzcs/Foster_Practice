using Engine;
using Foster.Framework;

namespace Content;

public interface IContent:ILifetime
{
    Target Target { get; }
    
}