using Microsoft.Xna.Framework;

using Aurora.Engine.Components;
using Aurora.Engine.Entities;

namespace Aurora.Engine.Systems;

public class AnimationSystem
{   
    public void Update(
            Entity entity,
            GameTime gameTime

        )

    {
        
        AnimationComponent? animation = 
            entity.GetComponent<AnimationComponent>();
        
        if ( animation == null ) 
            return;

        if (!animation.isMoving)
        {
            animation.CurrentFrame = 0;
            return;
        }

        animation.Timer += 
            (float)gameTime.ElapsedGameTime.TotalSeconds;


        if (animation.Timer >= animation.FrameTime)
        {
            animation.Timer = 0f;

            animation.CurrentFrame++;

            if( animation.CurrentFrame >= animation.FrameCount )
                animation.CurrentFrame = 0;
        }

        int row = animation.Direction switch
        {
            Direction.Down => 0,
            Direction.Up => 1,
            Direction.Left => 2,
            Direction.Right => 3,
            _ => 0
        };

        animation.SourceRectangle =
            new Rectangle(
                animation.CurrentFrame * 
                animation.FrameWidth,
                row *
                animation.FrameHeight,
                animation.FrameWidth,
                animation.FrameHeight

            );

       
    }

}