using Aurora.Engine.Entities;
using Aurora.Engine.Components;

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

        AnimationComponent? animation =
            entity.GetComponent<AnimationComponent>();

        Rectangle? source =
            animation?.SourceRectangle;


        _spriteBatch.Draw(
            entity.Texture,
            destination,
            source,
            Color.White
        );
    }

    public void Draw(
        IEnumerable<SpriteEntity> entities,
        Vector2 size 
        )
    {
        IEnumerable<SpriteEntity> sorted =
            entities
                .OrderBy(e => e.RenderLayer)
                .ThenBy(e => e.Depth);

        foreach (SpriteEntity entity in sorted)
        {
            Draw( entity, size );   
        }
    }
}
