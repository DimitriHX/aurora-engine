using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Tilemaps;

public class TileSet
{
    public Texture2D Texture { get; }
    public int FirstTileId { get; }
    public int TileCount => _sourceRectangles.Count;

    private readonly Dictionary<int, Rectangle> _sourceRectangles = new();

    public TileSet(Texture2D texture)
        : this(texture, 0)
    {
    }

    public TileSet(
        Texture2D texture,
        int firstTileId,
        int tileWidth = 0,
        int tileHeight = 0)
    {
        Texture = texture;
        FirstTileId = firstTileId;

        if (tileWidth > 0 && tileHeight > 0)
            RegisterGrid(tileWidth, tileHeight);
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

    public bool ContainsGlobalTile(int tileId)
    {
        int localTileId = tileId - FirstTileId;
        return localTileId >= 0 && localTileId < TileCount;
    }

    public Rectangle GetGlobalSourceRectangle(int tileId)
    {
        return GetSourceRectangle(tileId - FirstTileId);
    }

    private void RegisterGrid(int tileWidth, int tileHeight)
    {
        int columns = Texture.Width / tileWidth;
        int rows = Texture.Height / tileHeight;
        int tileId = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                RegisterTile(
                    tileId++,
                    new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight
                    )
                );
            }
        }
    }
}
