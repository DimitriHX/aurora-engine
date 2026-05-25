using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aurora.Engine.Assets;
using Aurora.Engine.Camera;
using Aurora.Engine.Components;
using Aurora.Engine.Entities;
using Aurora.Engine.Input;
using Aurora.Engine.Physics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Scenes;
using Aurora.Engine.Systems;
using Aurora.Engine.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aurora.Game.Scenes;

public class WorldScene : Scene
{
    // Declaraciones
    // Motor principal
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;

    // Render de texturas
    private readonly TextureRenderer _renderer;
    private readonly InputManager _input;
    private readonly Camera2D _camera;

    // Render los TileSet
    private TileMap _map = null!;
    private TileRenderer _tileRenderer = null!;
    private TileSet _tileSet = null!;
    private Texture2D _tileSetTexture = null!;

    // Asignamos la textura 2D
    private Texture2D _playerTexture = null;
    private Texture2D _pixel = null!;
    private AssetManager _assets = null!;

    // jugador xd, xd 
    private SpriteEntity _player = null!;
    private EntityRenderer _entityRenderer = null!;
    private MovementSystem _movementSystem = null!;
    private AnimationSystem _animationSystem = null!;
    private readonly List<SpriteEntity> _entities = [];

    // Asignamos el sistema de colision
    private CollisionSystem _collisionSystem = null!;
    private Vector2 _position = new(100, 100);

    private TileLayer _groundLayer = null!;
    private TileLayer _objectLayer = null!;
    private TileLayer _topLayer = null!;
    private TileLayer _baseLayer = null!;

    // Constructor de la escena del mundo inicial -- en desarrollo 
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
        // importante el orden de las asignaciones 
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _renderer = renderer;
        _input = input;
        _camera = camera;
        _assets = assets;
        // poner siempre el sistema de colision primero en caso de errores de carga
        // de modelos 
        _collisionSystem = new CollisionSystem();
        _movementSystem =
            new MovementSystem(_collisionSystem);
        _animationSystem = new AnimationSystem();
        _entityRenderer = new EntityRenderer(spriteBatch);
        _tileRenderer = new TileRenderer(spriteBatch);
    }
    
    // metodo principal de carga 
    public override void Load()
    {
        // player
        _player = new SpriteEntity();
        _entities.Add(_player);
        // cargamos la textura al personaje 
        _player.Texture = 
            _assets.LoadTexture("Textures/player");
        _player.Transform.Position =
            new Vector2(128, 128);
        // colisiones de player
        _player.BoundingBox.Width = 32;
        _player.BoundingBox.Height = 32;

        MovementComponent movement = new();
        _player.AddComponent(movement);

        AnimationComponent animation = new()
        {
            FrameWidth = 32,
            FrameHeight = 32,
            FrameCount = 2
        };

        animation.SourceRectangle = new Rectangle(
            0,
            0,
            animation.FrameWidth,
            animation.FrameHeight
            );

        _player.AddComponent(animation);

        // Cargamos el tile set
        _tileSetTexture =
            _assets.LoadTexture("Textures/tileset");
        _tileSet = new TileSet(_tileSetTexture);

        _tileSet.RegisterTile(
            0,
            new Rectangle(0, 0, 32, 32)
        );
        _tileSet.RegisterTile(
            1,
            new Rectangle(32, 0, 32, 32)
        );
   

        // Generamos el mapa si si si
        _map = new TileMap(32, 32, 32);
        // capas como cebollas / layers
        _baseLayer =
            new(
                "Base",
                _map.Width,
                _map.Height
                );

        _groundLayer =
            new(
                "Ground",
                _map.Width,
                _map.Height
                );

        _objectLayer =
            new(
                "Object",
                _map.Width,
                _map.Height
                );
        _topLayer =
            new(
                "Top",
                _map.Width,
                _map.Height
                );
        
        _map.AddLayer( _groundLayer );
        _map.AddLayer( _objectLayer );
        _map.AddLayer( _topLayer );
        _map.AddLayer( _baseLayer);

        
        


        for (int y = 0; y < _map.Height; y++)
        {
            for (int x = 0; x < _map.Width; x++)
            {
                _baseLayer.SetTile(x, y, 1);
                bool border =
                    x == 0 ||
                    y == 0 ||
                    x == _map.Width - 1 ||
                    y == _map.Height - 1;


                if (border) 
                {
                    _groundLayer.SetTile(x, y, 1);

                    _map.SetCollision(x, y, true);

                }
                    
                else
                {
                    _groundLayer.SetTile(x, y, 0);

                    _map.SetCollision(x,y,false);
                }
                    
            }
        }
    }

    // metodo de actualizacion para el movimiento
    public override void Update(GameTime gameTime)
    {
        // creamos el nuevo componente de movimiento
        MovementComponent? movement =
            _player.GetComponent<MovementComponent>();

        // si no se mueve por si acaso
        if (movement == null)
            return;

        // asignamos la velocidad estandar de movimiento
        

        AnimationComponent? animation =
            _player.GetComponent<AnimationComponent>();

        movement.Velocity = Vector2.Zero;

        if (_input.Left()) 
        {
            movement.Velocity.X = -1;

            animation.Direction =
               Direction.Left;
        }
            

        if (_input.Right()) 
        {
            movement.Velocity.X = 1;

            animation.Direction =
               Direction.Right;
        }
            

        if (_input.Up())
        {
            movement.Velocity.Y = -1;

            animation.Direction =
                Direction.Up;
        }
            

        if (_input.Down()) 
        {
            movement.Velocity.Y = 1;

            animation.Direction =
                Direction.Down;
        }
            

        if (movement.Velocity != Vector2.Zero)
        {
            movement.Velocity.Normalize();
        }

        animation.isMoving =
            movement.Velocity.LengthSquared() > 0;
        //Debug.WriteLine(movement.Velocity);
        _movementSystem.Update(
                _player,
                _map,
                gameTime
            );

        _animationSystem.Update(
                _player,
                gameTime
            );

        // juntamos la camara a el personaje 
        _camera.Position = 
            _player.Transform.Position -
            new Vector2(640, 360);

    }

    // metodo de dibujado de las entidades y los tiles
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(
            // fix, de las lineas del grip de tilemap 
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.Transform);
        // cargamos el render de el tile 
        _tileRenderer.DrawLayer(
            _map,
            _groundLayer,
            _tileSet            
            );
        _tileRenderer.DrawLayer(
            _map,
            _objectLayer,
            _tileSet
            );

        // Cargamos el render el personaje 
        _entityRenderer.Draw(
            _entities,
            new Vector2(32, 32)
            );

        //_tileRenderer.DrawLayer(
        //    _map,
        //    _topLayer,
        //    _tileSet
        //    );

        
        _spriteBatch.End();

    }
}
