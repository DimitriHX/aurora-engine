using Microsoft.Xna.Framework;

namespace Aurora.Engine.Components;

public class MovementComponent
{
    public Vector2 Velocity;
    public Vector2 TargetPosition;
    public float Speed = 128f;
    public bool IsMoving;
}
