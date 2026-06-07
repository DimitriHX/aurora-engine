using System.Text.Json;

using Aurora.Engine.Tilemaps;

namespace Aurora.Engine.Serialization;

public class MapLoader
{
    private const uint TiledFlipFlags = 0xE0000000;

    public TiledMapData LoadData(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(
                $"The Tiled map was not found: {path}",
                path
            );

        string json =
            File.ReadAllText(path);

        TiledMapData? data =
            JsonSerializer.Deserialize<TiledMapData>(json);

        if (data == null)
            throw new InvalidDataException(
                $"The Tiled map could not be deserialized: {path}"
            );

        Validate(data, path);

        return data;
    }

    public TileMap Load(string path)
    {
        return CreateMap(LoadData(path));
    }

    public TileMap CreateMap(TiledMapData data)
    {
        TileMap map =
            new(
                data.Width,
                data.Height,
                data.TileWidth
            );

        foreach (TiledLayerData layerData
            in data.Layers)
        {
            if (layerData.Type != "tilelayer")
                continue;

            TileLayer layer =
                new(
                    layerData.Name,
                    data.Width,
                    data.Height
                );

            layer.Visible = layerData.Visible;

            for (int i = 0; i < layerData.Data.Count; i++)
            {
                uint globalTileId = unchecked((uint)layerData.Data[i]);

                // Tiled stores flip information in the three highest GID bits.
                globalTileId &= ~TiledFlipFlags;
                layer.Tiles[i] = checked((int)globalTileId) - 1;
            }

            map.AddLayer(layer);
        }

        return map;
    }

    private static void Validate(TiledMapData data, string path)
    {
        if (data.Type != "map")
            throw new InvalidDataException(
                $"The file is not a Tiled map: {path}"
            );

        if (data.Infinite)
            throw new NotSupportedException(
                "Infinite Tiled maps are not supported yet."
            );

        if (data.Orientation != "orthogonal")
            throw new NotSupportedException(
                $"Tiled orientation '{data.Orientation}' is not supported."
            );

        if (data.Width <= 0 || data.Height <= 0 ||
            data.TileWidth <= 0 || data.TileHeight <= 0)
        {
            throw new InvalidDataException(
                "The Tiled map dimensions must be greater than zero."
            );
        }

        if (data.TileWidth != data.TileHeight)
            throw new NotSupportedException(
                "Aurora currently requires square tiles."
            );

        int expectedTileCount = data.Width * data.Height;

        foreach (TiledLayerData layer in data.Layers)
        {
            if (layer.Type != "tilelayer")
                continue;

            if (layer.Width != data.Width || layer.Height != data.Height)
                throw new NotSupportedException(
                    $"Layer '{layer.Name}' must match the map dimensions."
                );

            if (layer.Data.Count != expectedTileCount)
                throw new InvalidDataException(
                    $"Layer '{layer.Name}' contains {layer.Data.Count} tiles; " +
                    $"{expectedTileCount} were expected."
                );
        }
    }
}
