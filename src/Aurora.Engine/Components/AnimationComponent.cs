using Microsoft.Xna.Framework;

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
}