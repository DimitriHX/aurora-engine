using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Input;
using Aurora.Engine.Camera;
using Aurora.Engine.Scenes;
using Aurora.Engine.Tilemaps;
using Aurora.Engine.Assets;
using Aurora.Engine.Entities;
using System;
using Aurora.Engine.Physics;

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

    // jugador xd
    private SpriteEntity _player = null!;
    private EntityRenderer _entityRenderer = null!;

    private CollisionSystem _collisionSystem = null!;
    private Vector2 _position = new(100, 100);

    public WorldScene(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        TextureRenderer renderer,
        EntityRenderer entityRenderer,
        InputManager input,
        Camera2D camera,
        AssetManager assets
        )
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _renderer = renderer;
        _entityRenderer = new EntityRenderer(spriteBatch);
        _input = input;
        _camera = camera;
        _assets = assets;        
        _collisionSystem = new CollisionSystem();
        _tileRenderer = new TileRenderer(spriteBatch);
    }

    public override void Load()
    {
        _player = new SpriteEntity();
        _player.BoundingBox.Width = 32;
        _player.BoundingBox.Height = 32;
        _player.Texture = 
            _assets.LoadTexture("Textures/player");
        _player.Transform.Position =
            new Vector2(128, 128);

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
                bool border =
                    x == 0 ||
                    y == 0 ||
                    x == _map.Width - 1 ||
                    y == _map.Height - 1;


                if (border) 
                {
                    _map.SetTile(x, y, 1);

                    _map.SetCollision(x, y, true);

                }
                    
                else
                {
                    _map.SetTile(x, y, 0);

                    _map.SetCollision(x,y,false);
                }
                    
            }
        }


    }

    public override void Update(GameTime gameTime)
    {
        Vector2 newPosition =
            _player.Transform.Position;

        float speed = 200f;
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_input.Left())
            newPosition.X -= speed * delta;

        if (_input.Right())
            newPosition.X += speed * delta;

        if (_input.Up())
            newPosition.Y -= speed * delta;

        if (_input.Down())
            newPosition.Y += speed * delta;

        if (_collisionSystem.CanMove(
            _map,
            _player,
            newPosition
            ))
        {
            _player.Transform.Position = 
                newPosition;
        }

        _camera.Position = _player.Transform.Position - new Vector2(640, 360);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.Transform);

        _tileRenderer.Draw(_map, _tileSet);

        _entityRenderer.Draw(
            _player,
            new Vector2(32, 32)
        );

        _spriteBatch.End();

    }
}
