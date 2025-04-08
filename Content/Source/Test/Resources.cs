using Foster.Framework;

namespace Content.Test;

public class Resources
{
    public readonly SpriteFont? font;
    public readonly Texture? texture;
    public Batcher batcher;

    public Resources(SpriteFont font, Texture texture,Batcher batcher)
    {
        this.font = font;
        this.texture = texture;
        this.batcher = batcher;
    }
}