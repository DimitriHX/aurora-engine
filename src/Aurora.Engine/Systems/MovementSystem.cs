using Microsoft.Xna.Framework;

using Aurora.Engine.Components;
using Aurora.Engine.Entities;
using Aurora.Engine.Physics;
using Aurora.Engine.Tilemaps;

namespace Aurora.Engine.Systems;

public class MovementSystem
{
    private readonly CollisionSystem _collisionSystem;

    public MovementSystem(
        CollisionSystem collisionSystem)
    {
        _collisionSystem = collisionSystem;
    }

    public void Update(
            Entity entity,
            TileMap map,
            GameTime gameTime
        )
    {
        MovementComponent? movement =
            entity.GetComponent<MovementComponent>();

        if ( movement == null)
            return;

        float delta =
            (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 newPosition = 
            entity.Transform.Position + 
            movement.Velocity * 
            movement.Speed * 
            delta;

        if(_collisionSystem.CanMove(
            map,
            entity,
            newPosition
            ))
        {
            entity.Transform.Position = 
                newPosition;
        }
    }
}