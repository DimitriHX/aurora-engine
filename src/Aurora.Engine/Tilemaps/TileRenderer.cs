using System.Diagnostics;
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

    //public void Draw(
    //    TileMap map,
    //    TileSet tileSet
    //    )
    //{
    //    for (int y = 0; y < map.Height; y++)
    //    {
    //        for (int x = 0; x < map.Width; x++)
    //        {
    //            int tileId = map.GetTile(x, y);

    //            Rectangle source =
    //                tileSet.GetSourceRectangle(tileId);

    //            Rectangle destination = new(
    //                x * map.TileSize,
    //                y * map.TileSize,
    //                map.TileSize,
    //                map.TileSize
    //            );
    //            _spriteBatch.Draw(
    //                tileSet.Texture,
    //                destination,
    //                source,
    //                Color.White
    //            );
    //        }
    //    }
    //}

    
    public void DrawLayer(
        TileMap map,
        TileLayer layer,
        TileSet tileSet
        )
    {
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                
                int index =
                    x + y * map.Width;

                int tileId =
                    layer.Tiles[index];

                if (tileId < 0)
                    continue;

                Rectangle source =
                    tileSet.GetSourceRectangle(tileId);

                Rectangle destination = new(
                        x * map.TileSize,
                        y * map.TileSize,
                        map.TileSize,
                        map.TileSize
                    );

                if (source == Rectangle.Empty)
                    continue;

                _spriteBatch.Draw(
                        tileSet.Texture,
                        destination,
                        source,
                        Color.White
                    );
            }
        }
    }

    public void DrawLayer(
        TileMapResource resource,
        TileLayer layer)
    {
        if (!layer.Visible)
            return;

        TileMap map = resource.Map;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                int index = x + y * map.Width;
                int tileId = layer.Tiles[index];

                if (tileId < 0 ||
                    !resource.TryResolveTile(tileId, out TileSet? tileSet))
                {
                    continue;
                }

                Rectangle source =
                    tileSet!.GetGlobalSourceRectangle(tileId);

                if (source == Rectangle.Empty)
                    continue;

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
