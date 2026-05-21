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



}
