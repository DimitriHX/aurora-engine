using Aurora.Engine.Components;
using Aurora.Engine.Entities;
using Aurora.Engine.Physics;
using Aurora.Engine.Systems;
using Aurora.Engine.Tilemaps;
using Microsoft.Xna.Framework;

Entity entity = new();
entity.BoundingBox.Width = 32;
entity.BoundingBox.Height = 32;

MovementComponent movement = new();
entity.AddComponent(movement);

TileMap map = new(10, 10, 32);
MovementSystem movementSystem =
    new(new CollisionSystem());

bool started =
    movementSystem.TryStartMove(entity, map, Direction.Right);

GameTime frame = new(
    TimeSpan.Zero,
    TimeSpan.FromSeconds(0.1)
);

for (int i = 0; i < 3; i++)
    movementSystem.Update(entity, frame);

Assert(started, "The first grid step should start.");
Assert(
    entity.Transform.Position == new Vector2(32, 0),
    "The entity should finish exactly on the next tile."
);
Assert(
    !movement.IsMoving,
    "The movement should finish after reaching the target."
);

map.SetCollision(2, 0, true);

bool blocked =
    movementSystem.TryStartMove(entity, map, Direction.Right);

Assert(!blocked, "A solid tile should reject the movement.");
Assert(
    entity.Transform.Position == new Vector2(32, 0),
    "A blocked movement should not change the position."
);

Console.WriteLine("GRID_STEP_OK");
Console.WriteLine("COLLISION_BLOCK_OK");

static void Assert(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}
