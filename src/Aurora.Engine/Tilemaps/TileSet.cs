using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Tilemaps;

public class TileSet
{
    public Texture2D Texture { get;}

    private readonly Dictionary<int, Rectangle> _sourceRectangles = new();

    public TileSet(Texture2D texture) 
    { 
        Texture = texture;
    }

    public void RegisterTile(
        int tileId,
        Rectangle sourceRectangle)
    {
        _sourceRectangles[tileId] = sourceRectangle;
    }

    public Rectangle GetSourceRectangle( int tileId)
    {
        if (_sourceRectangles.TryGetValue(tileId,
            out Rectangle rectangle))          
        {
            return rectangle;
        }

        return Rectangle.Empty;
    }
}