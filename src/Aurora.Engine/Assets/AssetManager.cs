using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Engine.Assets;

public class AssetManager
{
    private readonly ContentManager _content;

    private readonly Dictionary<string, Texture2D> _textures = new();

    public AssetManager(ContentManager content)
    {
        _content = content;
    }

    public Texture2D LoadTexture(string assetName)
    {
        if (_textures.TryGetValue(assetName, out Texture2D? texture))
            return texture;

            texture = _content.Load<Texture2D>(assetName);
        

        _textures.Add(assetName, texture);

        return texture;
    }

    public void Unload()
    {
        _textures.Clear();
        _content.Unload();
    }
}