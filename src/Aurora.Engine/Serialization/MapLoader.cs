using System.Text.Json;

using Aurora.Engine.Tilemaps;

namespace Aurora.Engine.Serialization;

public class MapLoader
{
    public TileMap Load(string path)
    {
        string json = 
            File.ReadAllText(path);

        TileMapData? data = 
            JsonSerializer.Deserialize<TileMapData>(json);

        if (data == null)
            throw new Exception(
                "Fallo al cargarse el mapa wey"
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
                 i < layerData.Data.Count
                ; i++) 
            {
                layer.Tiles[i] =
                    layerData.Data[i] - 1;
            }

            map.AddLayer( layer );

        }

        return map;
    }
}