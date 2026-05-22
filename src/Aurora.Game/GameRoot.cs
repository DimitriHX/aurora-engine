using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Input;
using Aurora.Engine.Camera;
using System;
using Aurora.Engine.Scenes;
using Aurora.Game.Scenes;

namespace Aurora.Game;

public class GameRoot : Microsoft.Xna.Framework.Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null;
    private TextureRenderer _textureRenderer = null;
    private InputManager _input = null;
    private Camera2D _camera = null;
    private SceneManager _sceneManager = null;
    public GameRoot()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.Title = "Aurora Engine";

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
    }

    protected override void Initialize()
    {
        _input = new InputManager();

        _camera = new Camera2D();

        _sceneManager = new SceneManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _textureRenderer = new TextureRenderer(_spriteBatch);

        _sceneManager.ChangeScene(
            new WorldScene(
                GraphicsDevice,
                _spriteBatch,
                _textureRenderer,
                _input,
                _camera
            )
        );
    }


    protected override void Update(GameTime gameTime)
    {
        _input.Update();

        if (_input.ExitRequested())
            Exit();

        _sceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 20, 30));

        _sceneManager.Draw(gameTime);

        base.Draw(gameTime);
    }
}
