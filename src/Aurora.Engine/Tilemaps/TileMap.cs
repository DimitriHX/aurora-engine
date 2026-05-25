namespace Aurora.Engine.Tilemaps;

public class TileMap
{
    public int Width { get; }
    public int Height { get; }
    public int TileSize { get; }
    public List<TileLayer> Layers { get; } = [];

   

    public readonly bool[] _collisions;
    public TileMap(
        int width,
        int height,
        int tileSize
        )
    {
        Width = width;
        Height = height;
        TileSize = tileSize;
        _collisions = new bool[width * height];
    }
    public void AddLayer(TileLayer layer)
    {
        Layers.Add(layer);
    }

    public bool IsSolid(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return true;

        return _collisions[y * Width + x];
    }

    public void SetCollision(
        int x,
        int y,
        bool solid)
    {
        _collisions[y * Width + x] = solid;
    }
}