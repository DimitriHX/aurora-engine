using System;
using System.Collections.Generic;
using System.IO;
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
    private readonly SpriteBatch _spriteBatch;
    private readonly InputManager _input;
    private readonly Camera2D _camera;
    private readonly AssetManager _assets;
    private readonly TileRenderer _tileRenderer;
    private readonly EntityRenderer _entityRenderer;
    private readonly MovementSystem _movementSystem;
    private readonly AnimationSystem _animationSystem;
    private readonly List<SpriteEntity> _entities = [];

    private TileMapResource _world = null!;
    private TileMap _map = null!;
    private SpriteEntity _player = null!;
    private TileLayer _groundLayer = null!;
    private TileLayer _objectLayer = null!;
    private TileLayer _topLayer = null!;
    private TileLayer _baseLayer = null!;

    public WorldScene(
        SpriteBatch spriteBatch,
        InputManager input,
        Camera2D camera,
        AssetManager assets)
    {
        _spriteBatch = spriteBatch;
        _input = input;
        _camera = camera;
        _assets = assets;

        _movementSystem =
            new MovementSystem(new CollisionSystem());
        _animationSystem = new AnimationSystem();
        _entityRenderer = new EntityRenderer(spriteBatch);
        _tileRenderer = new TileRenderer(spriteBatch);
    }

    public override void Load()
    {
        LoadMap();
        LoadPlayer();
        CenterCamera();
    }

    public override void Update(GameTime gameTime)
    {
        UpdatePlayerMovement(gameTime);
        _animationSystem.Update(_player, gameTime);
        CenterCamera();
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.Transform
        );

        DrawLayer(_baseLayer);
        DrawLayer(_groundLayer);
        DrawLayer(_objectLayer);

        _entityRenderer.Draw(
            _entities,
            new Vector2(_map.TileSize)
        );

        DrawLayer(_topLayer);
        _spriteBatch.End();
    }

    private void LoadMap()
    {
        TileMapManager tileMapManager = new(_assets);

        _world = tileMapManager.Add(
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

        _map = _world.Map;
        _baseLayer = GetRequiredLayer("Base");
        _groundLayer = GetRequiredLayer("Ground");
        _objectLayer = GetRequiredLayer("Objects");
        _topLayer = GetRequiredLayer("Top");
    }

    private void LoadPlayer()
    {
        _player = new SpriteEntity
        {
            Texture = _assets.LoadTexture("Textures/player")
        };

        _player.Transform.Position = new Vector2(128, 128);
        _player.BoundingBox.Width = _map.TileSize;
        _player.BoundingBox.Height = _map.TileSize;
        _player.AddComponent(new MovementComponent());
        _player.AddComponent(
            new AnimationComponent
            {
                FrameWidth = 32,
                FrameHeight = 32,
                FrameCount = 2,
                SourceRectangle = new Rectangle(0, 0, 32, 32)
            }
        );

        _entities.Add(_player);
    }

    private void UpdatePlayerMovement(GameTime gameTime)
    {
        MovementComponent movement =
            GetRequiredComponent<MovementComponent>(_player);
        AnimationComponent animation =
            GetRequiredComponent<AnimationComponent>(_player);

        _movementSystem.Update(_player, gameTime);

        if (!movement.IsMoving &&
            TryGetMovementDirection(out Direction direction))
        {
            animation.Direction = direction;
            _movementSystem.TryStartMove(_player, _map, direction);
        }

        animation.isMoving = movement.IsMoving;
    }

    private bool TryGetMovementDirection(out Direction direction)
    {
        if (_input.LeftPressed())
            return SetDirection(Direction.Left, out direction);

        if (_input.RightPressed())
            return SetDirection(Direction.Right, out direction);

        if (_input.UpPressed())
            return SetDirection(Direction.Up, out direction);

        if (_input.DownPressed())
            return SetDirection(Direction.Down, out direction);

        if (_input.Left())
            return SetDirection(Direction.Left, out direction);

        if (_input.Right())
            return SetDirection(Direction.Right, out direction);

        if (_input.Up())
            return SetDirection(Direction.Up, out direction);

        if (_input.Down())
            return SetDirection(Direction.Down, out direction);

        direction = Direction.Down;
        return false;
    }

    private static bool SetDirection(
        Direction value,
        out Direction direction)
    {
        direction = value;
        return true;
    }

    private TileLayer GetRequiredLayer(string name)
    {
        TileLayer layer = _map.GetLayer(name);

        if (layer == null)
        {
            throw new InvalidDataException(
                $"Required tilemap layer '{name}' was not found."
            );
        }

        return layer;
    }

    private static T GetRequiredComponent<T>(Entity entity)
        where T : class
    {
        T component = entity.GetComponent<T>();

        if (component == null)
        {
            throw new InvalidOperationException(
                $"Entity requires component '{typeof(T).Name}'."
            );
        }

        return component;
    }

    private void DrawLayer(TileLayer layer)
    {
        _tileRenderer.DrawLayer(_world, layer);
    }

    private void CenterCamera()
    {
        _camera.Position =
            _player.Transform.Position -
            new Vector2(640, 360);
    }
}
