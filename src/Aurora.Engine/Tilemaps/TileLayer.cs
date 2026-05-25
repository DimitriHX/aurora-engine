namespace Aurora.Engine.Tilemaps;
public class TileLayer {
    public int[] Tiles { get; }
    
    public bool Visible = true;
    public string Name { get; }
    private readonly int _width;
    public TileLayer(
        string name,
        int width,
        int height
        )
    {
        Name = name;
        _width = width;
        Tiles = new int[width * height];
    }
   
    public int GetTiles(
        int x, 
        int y
        )
    {
        return Tiles[
            x + y * _width
            ];
    }

    public void SetTile(
        int x,
        int y,
        int value
        )
    {
        Tiles[
            x + y * _width
            ] = value;
    }

}