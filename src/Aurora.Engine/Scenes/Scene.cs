using Microsoft.Xna.Framework;

namespace Aurora.Engine.Scenes;

public abstract class Scene
{
    public virtual void Initialize()
    {
    }

    public virtual void Load()
    {
    }

    public virtual void Unload()
    {

    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }
}