namespace Aurora.Engine.Tilemaps;

public class TileMap
{
    public int Width { get; }
    public int Height { get; }
    public int TileSize { get; }
    public int[] Tiles { get; }

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

        Tiles = new int[width * height];
        _collisions = new bool[width * height];
    }

    public int GetTile(int x, int y)
    {
        return Tiles[y * Width + x];
    }

    public void SetTile(int x, int y, int value)
    {
        Tiles[y * Width + x] = value;
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