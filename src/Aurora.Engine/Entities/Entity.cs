namespace Aurora.Engine.Entities;

public class Entity
{
    public Transform Transform { get; } = new();
    public BoundingBox BoundingBox { get; set; } = new();
}