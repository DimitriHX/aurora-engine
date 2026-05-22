using Microsoft.Xna.Framework;

namespace Aurora.Engine.Scenes;

public class SceneManager
{
    public Scene? ActiveScene { get; private set; }

    public void ChangeScene(Scene newScene)
    {
        ActiveScene?.Unload();

        ActiveScene = newScene;

        ActiveScene.Initialize();
        ActiveScene.Load();
    }

    public void Update(GameTime gameTime)
    {
        ActiveScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        ActiveScene?.Draw(gameTime);
    }
}