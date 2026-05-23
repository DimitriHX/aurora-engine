using Microsoft.Xna.Framework;

namespace Aurora.Engine.Camera;

public class Camera2D
{
    public Vector2 Position { get; set; }

    public Matrix Transform => 
        Matrix.CreateTranslation(
            -(int)Position.X,
            -(int)Position.Y,
            0f
        );
}