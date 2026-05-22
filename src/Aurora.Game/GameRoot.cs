using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Input;
using Aurora.Engine.Camera;
using System;

namespace Aurora.Game;

public class GameRoot : Microsoft.Xna.Framework.Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null;
    private TextureRenderer _textureRenderer = null;
    private InputManager _input = null;
    private Camera2D _camera = null;
    private Texture2D _pixel = null;
    private Vector2 _position = new(100, 100);
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _textureRenderer = new TextureRenderer(_spriteBatch);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);

        _pixel.SetData(new[] { Color.White });
    }


    protected override void Update(GameTime gameTime)
    {
        _input.Update();

        if (_input.ExitRequested())
            Exit();
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 20, 30));

        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _textureRenderer.Draw(
            _pixel,
            _position,
            new Vector2(32,32),
            Color.CornflowerBlue
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
