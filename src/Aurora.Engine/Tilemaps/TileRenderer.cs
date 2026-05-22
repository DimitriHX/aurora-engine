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
        Texture2D texture
        )
    {
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                int tile = map.GetTile(x, y);

                Color color = tile switch
                {
                    0 => new Color(50, 120, 50),
                    1 => new Color(30, 30, 160),
                    _ => Color.Magenta
                };

                Rectangle rectangle = new(
                    x * map.TileSize,
                    y * map.TileSize,
                    map.TileSize,
                    map.TileSize
                );

                _spriteBatch.Draw(
                    texture,
                    rectangle,
                    color
                );
            }
        }
    }
}