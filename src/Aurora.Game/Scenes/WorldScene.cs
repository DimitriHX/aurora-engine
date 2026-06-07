using System;
using System.Collections.Generic;
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
    private TileMapResource _tileMapResource = null!;
    private TileMapManager _tileMapManager = null!;

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

        _tileMapManager = new TileMapManager(_assets);
        _tileMapResource = _tileMapManager.Add(
            "world",
            new TileMapDefinition(
                "Content/Maps/test_map.json",
                new Dictionary<string, string>
                {
                    ["../tileset4.tsx"] = "Textures/tileset"
                }
            )
            {
                CollisionLayer = "Collision"
            }
        );

        _map = _tileMapResource.Map;
        _baseLayer = GetRequiredLayer("Base");
        _groundLayer = GetRequiredLayer("Ground");
        _objectLayer = GetRequiredLayer("Objects");
        _topLayer = GetRequiredLayer("Top");
    }

    private TileLayer GetRequiredLayer(string name)
    {
        TileLayer layer = _map.GetLayer(name);

        if (layer == null)
        {
            throw new System.IO.InvalidDataException(
                $"Required tilemap layer '{name}' was not found."
            );
        }

        return layer;
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
            _tileMapResource,
            _baseLayer
        );
        _tileRenderer.DrawLayer(
            _tileMapResource,
            _groundLayer
        );
        _tileRenderer.DrawLayer(
            _tileMapResource,
            _objectLayer
        );

        // Cargamos el render el personaje 
        _entityRenderer.Draw(
            _entities,
            new Vector2(32, 32)
            );

        _tileRenderer.DrawLayer(
            _tileMapResource,
            _topLayer
        );

        
        _spriteBatch.End();

    }
}
