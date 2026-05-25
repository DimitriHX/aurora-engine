using System.Text.Json.Serialization;

namespace Aurora.Engine.Serialization;

public class TileMapData
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
}

public class TiledLayerData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("data")]
    public List<int> Data { get; set; } = [];
}