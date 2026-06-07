using Aurora.Engine.Assets;
using Aurora.Engine.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Tilemaps;

public sealed class TileMapManager
{
    private readonly AssetManager _assets;
    private readonly MapLoader _loader;
    private readonly Dictionary<string, TileMapResource> _maps = [];

    public TileMapManager(
        AssetManager assets,
        MapLoader? loader = null)
    {
        _assets = assets;
        _loader = loader ?? new MapLoader();
    }

    public TileMapResource Add(
        string name,
        TileMapDefinition definition)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "A tilemap name is required.",
                nameof(name)
            );

        if (_maps.ContainsKey(name))
            throw new InvalidOperationException(
                $"A tilemap named '{name}' is already loaded."
            );

        TiledMapData data = _loader.LoadData(definition.MapPath);
        TileMap map = _loader.CreateMap(data);
        List<TileSet> tileSets = CreateTileSets(data, definition);

        if (!string.IsNullOrWhiteSpace(definition.CollisionLayer))
            ApplyCollisions(map, definition.CollisionLayer);

        TileMapResource resource = new(map, tileSets);
        _maps.Add(name, resource);

        return resource;
    }

    public TileMapResource Get(string name)
    {
        if (_maps.TryGetValue(name, out TileMapResource? resource))
            return resource;

        throw new KeyNotFoundException(
            $"The tilemap '{name}' has not been loaded."
        );
    }

    public bool Remove(string name)
    {
        return _maps.Remove(name);
    }

    public void Clear()
    {
        _maps.Clear();
    }

    private List<TileSet> CreateTileSets(
        TiledMapData data,
        TileMapDefinition definition)
    {
        if (data.Tilesets.Count == 0)
            throw new InvalidDataException(
                "The Tiled map does not reference any tilesets."
            );

        List<TileSet> tileSets = [];

        foreach (TiledTilesetData tiledTileSet in data.Tilesets)
        {
            if (!definition.TilesetAssets.TryGetValue(
                    tiledTileSet.Source,
                    out string? assetName))
            {
                throw new InvalidDataException(
                    $"No MonoGame asset was configured for Tiled tileset " +
                    $"'{tiledTileSet.Source}'."
                );
            }

            Texture2D texture = _assets.LoadTexture(assetName);

            tileSets.Add(
                new TileSet(
                    texture,
                    tiledTileSet.FirstGid - 1,
                    data.TileWidth,
                    data.TileHeight
                )
            );
        }

        return tileSets;
    }

    private static void ApplyCollisions(
        TileMap map,
        string collisionLayerName)
    {
        TileLayer? collisionLayer =
            map.GetLayer(collisionLayerName);

        if (collisionLayer == null)
            throw new InvalidDataException(
                $"Collision layer '{collisionLayerName}' was not found."
            );

        collisionLayer.Visible = false;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                map.SetCollision(
                    x,
                    y,
                    collisionLayer.GetTiles(x, y) >= 0
                );
            }
        }
    }
}
