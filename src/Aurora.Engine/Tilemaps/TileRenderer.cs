using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Tilemaps;

public class TileRenderer
{
    private readonly SpriteBatch _spriteBatch;

    public TileRenderer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public void Draw(
        TileMap map,
        TileSet tileSet
        )
    {
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                int tileId = map.GetTile(x, y);

                Rectangle source =
                    tileSet.GetSourceRectangle(tileId);

                Rectangle destination = new(
                    x * map.TileSize,
                    y * map.TileSize,
                    map.TileSize,
                    map.TileSize
                );
                _spriteBatch.Draw(
                    tileSet.Texture,
                    destination,
                    source,
                    Color.White
                );
            }
        }
    }
}