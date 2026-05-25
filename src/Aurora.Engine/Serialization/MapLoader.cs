using System.Text.Json;

using Aurora.Engine.Tilemaps;

namespace Aurora.Engine.Serialization;

public class MapLoader
{
    public TileMap Load(string path)
    {
        string json =
            File.ReadAllText(path);

        TiledMapData? data =
            JsonSerializer.Deserialize<TiledMapData>(json);

        if (data == null)
            throw new Exception(
                "Failed to load map."
            );

        TileMap map =
            new(
                data.Width,
                data.Height,
                data.TileWidth
            );



        foreach (TiledLayerData layerData
            in data.Layers)
        {
           
            TileLayer layer =
                new(
                    layerData.Name,
                    data.Width,
                    data.Height
                );

            for (int i = 0;
                 i < layerData.Data.Count;
                 i++)
            {
                layer.Tiles[i] =
                    layerData.Data[i] - 1;
            }

            map.AddLayer(layer);

        }

        return map;
    }
}