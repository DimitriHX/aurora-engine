using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Aurora.Engine.Rendering;
using Aurora.Engine.Input;
using Aurora.Engine.Camera;
using Aurora.Engine.Scenes;

namespace Aurora.Game.Scenes;

public class WorldScene : Scene
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;

    private readonly TextureRenderer _renderer;
    private readonly InputManager _input;
    private readonly Camera2D _camera;

    private Texture2D _pixel = null;

    private Vector2 _position = new(100, 100);

    public WorldScene(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        TextureRenderer renderer,
        InputManager input,
        Camera2D camera
        )
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _renderer = renderer;   
        _input = input;
        _camera = camera;   
    }

    public override void Load()
    {
        _pixel = new Texture2D(_graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
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
        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _renderer.Draw(
            _pixel,
            _position,
            new Vector2(32,32),
            Color.CornflowerBlue
        );

        _spriteBatch.End();

    }
}
