using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoord;

    public Vertex(Vector3 Position, Vector3 Normal, Vector2 TexCoord)
    {
        this.Position = Position;
        this.Normal = Normal;
        this.TexCoord = TexCoord;
    }
}