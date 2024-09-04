using OpenTK.Windowing.Desktop;

namespace VoxelGame.Engine;

public abstract class Scene
{
    public MainWindow OwningWindow;
    public abstract void Load();
    public abstract void Unload();

    public abstract void Update(float DeltaTime);
    public abstract void Render(float DeltaTime);

}