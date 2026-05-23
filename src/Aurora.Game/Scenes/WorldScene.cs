using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Input;
using Aurora.Engine.Camera;
using Aurora.Engine.Scenes;
using Aurora.Engine.Tilemaps;
using Aurora.Engine.Assets;

namespace Aurora.Game.Scenes;

public class WorldScene : Scene
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;

    private readonly TextureRenderer _renderer;
    private readonly InputManager _input;
    private readonly Camera2D _camera;

    private TileMap _map = null!;
    private TileRenderer _tileRenderer = null!;
    private TileSet _tileSet = null!;
    private Texture2D _tileSetTexture = null!;


    private Texture2D _playerTexture = null;
    private Texture2D _pixel = null!;
    private AssetManager _assets = null!;

    private Vector2 _position = new(100, 100);

    public WorldScene(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        TextureRenderer renderer,
        InputManager input,
        Camera2D camera,
        AssetManager assets
        )
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _renderer = renderer;   
        _input = input;
        _camera = camera;
        _assets = assets;
        _tileRenderer = new TileRenderer(spriteBatch);
    }

    public override void Load()
    {
        _pixel = new Texture2D(_graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _playerTexture = 
            _assets.LoadTexture("Textures/player");
        _tileSetTexture =
            _assets.LoadTexture("Textures/tileset");
        _tileSet = new TileSet(_tileSetTexture);

        _tileSet.RegisterTile(
            0,
            new Rectangle(0, 0, 32, 32)
        );

        _tileSet.RegisterTile(
            1,
            new Rectangle(32,0,32,32)
        );


        _map = new TileMap(32, 32, 32);

        for (int y = 0; y < _map.Height; y++)
        {
            for (int x = 0; x < _map.Height; x++)
            {
                if (y < 10)
                    _map.SetTile(x, y, 1);
                else
                    _map.SetTile(x, y, 0);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        float speed = 200f;
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_input.Left())
            _position.X -= speed * delta;

        if (_input.Right())
            _position.X += speed * delta;

        if (_input.Up())
            _position.Y -= speed * delta;

        if (_input.Down())
            _position.Y += speed * delta;

        _camera.Position = _position - new Vector2(640, 360);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.Transform);

        _tileRenderer.Draw(_map, _tileSet);


        _renderer.Draw(
            _playerTexture,
            _position,
            new Vector2(32,32),
            Color.White
        );

        _spriteBatch.End();

    }
}
