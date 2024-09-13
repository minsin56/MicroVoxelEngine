using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector3 Color;
    public Vector2 TexCoord;

    public Vertex(Vector3 Position, Vector3 Normal, Vector2 TexCoord)
    {
        this.Position = Position;
        this.Normal = Normal;
        this.TexCoord = TexCoord;
    }
        public Vertex(Vector3 Position, Vector3 Normal, Vector3 Color, Vector2 TexCoord)
    {
        this.Position = Position;
        this.Normal = Normal;
        this.Color = Color;
        this.TexCoord = TexCoord;
    }

        public static readonly int SizeInBytes = Vector3.SizeInBytes * 3 + Vector2.SizeInBytes;

}