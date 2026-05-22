namespace Aurora.Engine.Tilemaps;

public class TileMap
{
    public int Width { get; }
    public int Height { get; }
    public int TileSize { get; }
    public int[] Tiles { get; }
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
    }

    public int GetTile(int x, int y)
    {
        return Tiles[y * Width + x];
    }

    public void SetTile(int x, int y, int value)
    {
        Tiles[y * Width + x] = value;
    }
}