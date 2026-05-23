namespace Aurora.Engine.Entities;

public class Entity
{
    public Transform Transform { get; } = new();
    public BoundingBox BoundingBox { get; set; } = new();

    private readonly Dictionary<Type, object> _components = new();

    public void AddComponent<T>(T component)
        where T : class
    {
        _components[typeof(T)] = component; 
    } 

    public T? GetComponent<T>()
        where T : class
    {
        return _components.TryGetValue(typeof(T), out object? component)
            ? component as T
            : null;
    }
        
}