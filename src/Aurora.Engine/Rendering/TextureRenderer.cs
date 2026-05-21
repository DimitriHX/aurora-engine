using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Rendering;

public class TextureRenderer
{
    private readonly SpriteBatch _spriteBatch;

    public TextureRenderer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public void Draw(
        Texture2D texture,
        Vector2 position,
        Vector2 size,
        Color color
        )
    {
        Rectangle rectangle = new(
            (int)position.X,
            (int)position.Y,
            (int)size.X,
            (int)size.Y
            );

        _spriteBatch.Draw(texture, rectangle, color);
    }

}
