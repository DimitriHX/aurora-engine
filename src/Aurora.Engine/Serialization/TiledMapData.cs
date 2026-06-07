using System.Text.Json.Serialization;

namespace Aurora.Engine.Serialization;

public class TiledMapData
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("tilewidth")]
    public int TileWidth { get; set; }

    [JsonPropertyName("tileheight")]
    public int TileHeight { get; set; }

    [JsonPropertyName("layers")]
    public List<TiledLayerData> Layers { get; set; } = [];

    [JsonPropertyName("tilesets")]
    public List<TiledTilesetData> Tilesets { get; set; } = [];

    [JsonPropertyName("infinite")]
    public bool Infinite { get; set; }

    [JsonPropertyName("orientation")]
    public string Orientation { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
}

public class TiledLayerData
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("data")]
    public List<int> Data { get; set; } = [];

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("visible")]
    public bool Visible { get; set; } = true;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class TiledTilesetData
{
    [JsonPropertyName("firstgid")]
    public int FirstGid { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; } = "";
}
