using Aurora.Engine.Components;
using Aurora.Engine.Entities;
using Aurora.Engine.Physics;
using Aurora.Engine.Tilemaps;
using Microsoft.Xna.Framework;

namespace Aurora.Engine.Systems;

public class MovementSystem
{
    private readonly CollisionSystem _collisionSystem;

    public MovementSystem(CollisionSystem collisionSystem)
    {
        _collisionSystem = collisionSystem;
    }

    public bool TryStartMove(
        Entity entity,
        TileMap map,
        Direction direction)
    {
        MovementComponent? movement =
            entity.GetComponent<MovementComponent>();

        if (movement == null || movement.IsMoving)
            return false;

        Vector2 offset = direction switch
        {
            Direction.Left => new Vector2(-map.TileSize, 0),
            Direction.Right => new Vector2(map.TileSize, 0),
            Direction.Up => new Vector2(0, -map.TileSize),
            Direction.Down => new Vector2(0, map.TileSize),
            _ => Vector2.Zero
        };

        Vector2 targetPosition =
            entity.Transform.Position + offset;

        if (!_collisionSystem.CanMove(map, entity, targetPosition))
            return false;

        movement.TargetPosition = targetPosition;
        movement.Velocity = Vector2.Normalize(offset);
        movement.IsMoving = true;

        return true;
    }

    public void Update(
        Entity entity,
        GameTime gameTime)
    {
        MovementComponent? movement =
            entity.GetComponent<MovementComponent>();

        if (movement == null || !movement.IsMoving)
            return;

        float distance =
            movement.Speed *
            (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 remaining =
            movement.TargetPosition - entity.Transform.Position;

        if (remaining.LengthSquared() <= distance * distance)
        {
            entity.Transform.Position = movement.TargetPosition;
            movement.Velocity = Vector2.Zero;
            movement.IsMoving = false;
            return;
        }

        entity.Transform.Position += movement.Velocity * distance;
    }
}
