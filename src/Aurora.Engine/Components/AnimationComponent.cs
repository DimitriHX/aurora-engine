using Microsoft.Xna.Framework;
using Aurora.Engine.Entities;

namespace Aurora.Engine.Components;

public class AnimationComponent
{
    public int FrameWidth;
    public int FrameHeight;
    public int CurrentFrame;
    public int FrameCount;
    public float FrameTime = 0.15f;
    public float Timer;
    public Rectangle SourceRectangle;
    public Direction Direction = Direction.Down;
    public bool isMoving;
}