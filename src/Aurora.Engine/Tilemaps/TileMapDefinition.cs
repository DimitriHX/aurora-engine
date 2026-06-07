namespace Aurora.Engine.Tilemaps;

public sealed class TileMapDefinition
{
    public string MapPath { get; }
    public string? CollisionLayer { get; init; }
    public IReadOnlyDictionary<string, string> TilesetAssets { get; }

    public TileMapDefinition(
        string mapPath,
        IReadOnlyDictionary<string, string> tilesetAssets)
    {
        MapPath = mapPath;
        TilesetAssets = tilesetAssets;
    }
}
