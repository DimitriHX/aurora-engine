using Microsoft.Xna.Framework;

namespace Aurora.Engine.Entities;

public class BoundingBox
{
    public int Width;
    public int Height;

    public Rectangle GetBounds(Vector2 position)
    {
        return new Rectangle(
            (int)position.X,
            (int)position.Y,
            Width,
            Height
        ); 
    }
}