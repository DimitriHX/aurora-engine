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

        animation.Timer += 
            (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (animation.Timer >= animation.FrameTime)
        {
            animation.Timer = 0f;

            animation.CurrentFrame++;

            if( animation.CurrentFrame >= animation.FrameCount )
                animation.CurrentFrame = 0;
        }

        animation.SourceRectangle =
            new Rectangle(
                animation.CurrentFrame * 
                animation.FrameWidth,
                0,
                animation.FrameWidth,
                animation.FrameHeight

            );

       
    }

}