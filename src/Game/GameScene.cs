using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxelGame.Engine;
using VoxelGame.Voxel;

namespace VoxelGame.Game;

class GameScene : Scene
{
    float Time;
    Texture2D Tex;

    World World;

    public GameScene()
    {
        
    }

    public override void Load()
    {
        World = new World();
    }

    public override void Render(float DeltaTime)
    {
        World.Draw();
    }

    public override void Unload()
    {

    }
    public override void Update(float DeltaTime)
    {
        Time += DeltaTime;
        World.Update(DeltaTime);
    }
}
