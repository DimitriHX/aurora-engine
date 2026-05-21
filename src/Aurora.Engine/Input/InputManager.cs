using Microsoft.Xna.Framework.Input;
namespace Aurora.Engine.Input;

public class InputManager
{
    private KeyboardState _keyboardState;

    public void Update()
    {
        _keyboardState = Keyboard.GetState();
    }

    public bool Up()
        => _keyboardState.IsKeyDown(Keys.W);

    public bool Down()
        => _keyboardState.IsKeyDown(Keys.S);

    public bool Left()
        => _keyboardState.IsKeyDown(Keys.A);

    public bool Right()
        => _keyboardState.IsKeyDown(Keys.D);

    public bool ExitRequested()
        => _keyboardState.IsKeyDown(Keys.Escape);
}
