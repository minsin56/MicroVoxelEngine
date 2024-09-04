namespace VoxelGame.Engine;

public abstract class Texture
{
    public int Handle;

    public abstract void Load(string Path);
    public abstract void Bind();
}