using Microsoft.Xna.Framework.Input;

namespace Aurora.Engine.Input;

public class InputManager
{
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;

    public void Update()
    {
        _previousKeyboardState = _keyboardState;
        _keyboardState = Keyboard.GetState();
    }

    public bool Up()
        => IsDown(Keys.W, Keys.Up);

    public bool Down()
        => IsDown(Keys.S, Keys.Down);

    public bool Left()
        => IsDown(Keys.A, Keys.Left);

    public bool Right()
        => IsDown(Keys.D, Keys.Right);

    public bool UpPressed()
        => IsPressed(Keys.W, Keys.Up);

    public bool DownPressed()
        => IsPressed(Keys.S, Keys.Down);

    public bool LeftPressed()
        => IsPressed(Keys.A, Keys.Left);

    public bool RightPressed()
        => IsPressed(Keys.D, Keys.Right);

    public bool ExitRequested()
        => _keyboardState.IsKeyDown(Keys.Escape);

    private bool IsDown(Keys primary, Keys secondary)
    {
        return _keyboardState.IsKeyDown(primary) ||
               _keyboardState.IsKeyDown(secondary);
    }

    private bool IsPressed(Keys primary, Keys secondary)
    {
        bool isDown = IsDown(primary, secondary);
        bool wasDown =
            _previousKeyboardState.IsKeyDown(primary) ||
            _previousKeyboardState.IsKeyDown(secondary);

        return isDown && !wasDown;
    }
}
