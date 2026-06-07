namespace Aurora.Engine.Tilemaps;

public sealed class TileMapResource
{
    private readonly List<TileSet> _tileSets;

    public TileMap Map { get; }
    public IReadOnlyList<TileSet> TileSets => _tileSets;

    public TileMapResource(TileMap map, IEnumerable<TileSet> tileSets)
    {
        Map = map;
        _tileSets = tileSets
            .OrderByDescending(tileSet => tileSet.FirstTileId)
            .ToList();
    }

    public bool TryResolveTile(
        int globalTileId,
        out TileSet? tileSet)
    {
        tileSet = _tileSets.FirstOrDefault(
            candidate => candidate.ContainsGlobalTile(globalTileId)
        );

        return tileSet != null;
    }
}
