using Microsoft.Xna.Framework;
using Aurora.Engine.Entities;
using Aurora.Engine.Tilemaps;

namespace Aurora.Engine.Physics;

public class CollisionSystem
{
    public bool CanMove(
            TileMap map,
            Entity entity,
            Vector2 newPosition
        )
    {
        
        Rectangle bounds =
            entity.BoundingBox.GetBounds( newPosition );

        int left = 
            bounds.Left / map.TileSize;

        int right = 
            (bounds.Right - 1 ) / map.TileSize;

        int top = 
            bounds.Top / map.TileSize;

        int bottom =
            (bounds.Bottom - 1) / map.TileSize;

        return
            !map.IsSolid(left, top) &&
            !map.IsSolid(right, top) &&
            !map.IsSolid(left, bottom) &&
            !map.IsSolid(right, bottom);
    }
}