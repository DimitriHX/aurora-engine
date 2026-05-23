using Aurora.Engine.Entities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Aurora.Engine.Rendering;
public class EntityRenderer
{
    private readonly SpriteBatch _spriteBatch;

    public EntityRenderer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public void Draw(
        SpriteEntity entity,
        Vector2 size
        )
    {
        Rectangle destination = new(
            (int)entity.Transform.Position.X,
            (int)entity.Transform.Position.Y,
            (int)size.X,
            (int)size.Y
        );

        _spriteBatch.Draw(
            entity.Texture,
            destination,
            Color.White
        );
    }
}
