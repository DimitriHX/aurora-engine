using Aurora.Engine.Components;
using Aurora.Engine.Entities;
using Microsoft.Xna.Framework;

namespace Aurora.Engine.Systems;

public class AnimationSystem
{
    public void Update(
        Entity entity,
        GameTime gameTime)
    {
        AnimationComponent? animation =
            entity.GetComponent<AnimationComponent>();

        if (animation == null)
            return;

        int row = animation.Direction switch
        {
            Direction.Down => 0,
            Direction.Left => 1,
            Direction.Right => 2,
            Direction.Up => 3,
            _ => 0
        };

        if (!animation.isMoving)
        {
            animation.CurrentFrame = 0;
            animation.Timer = 0f;
        }
        else
        {
            animation.Timer +=
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animation.Timer >= animation.FrameTime)
            {
                animation.Timer = 0f;
                animation.CurrentFrame++;

                if (animation.CurrentFrame >= animation.FrameCount)
                    animation.CurrentFrame = 0;
            }
        }

        animation.SourceRectangle = new Rectangle(
            animation.CurrentFrame * animation.FrameWidth,
            row * animation.FrameHeight,
            animation.FrameWidth,
            animation.FrameHeight
        );
    }
}
