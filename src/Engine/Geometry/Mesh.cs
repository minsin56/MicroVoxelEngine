using OpenTK.Graphics.OpenGL4;
using ObjectTK.Buffers;
using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public class Mesh
{
    public Vertex[] Vertices;
    public uint[] Indices;
    
    public int VAO;

    public void GenerateBuffers()
    {
        VAO = GL.GenVertexArray();
        GL.BindVertexArray(VAO);

        int VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer,VBO);
        GL.BufferData(BufferTarget.ArrayBuffer,Vertices.Length * Vertex.SizeInBytes,Vertices,BufferUsageHint.StaticDraw);

        int EBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer,EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer,Indices.Length * sizeof(uint),Indices,BufferUsageHint.StaticDraw);


        // Set the position attribute (location = 0 in shader)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
        GL.EnableVertexAttribArray(0);

        // Set the color attribute (location = 1 in shader)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes);
        GL.EnableVertexAttribArray(1);

        // Set the normal attribute (location = 2 in shader)
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 2 * Vector3.SizeInBytes);
        GL.EnableVertexAttribArray(2);

        // Set the UV attribute (location = 3 in shader)
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 3 * Vector3.SizeInBytes);
        GL.EnableVertexAttribArray(3);

        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(VAO);
        GL.DrawElements(PrimitiveType.Triangles,Indices.Length,DrawElementsType.UnsignedInt,0);
    }

    public static Mesh GenerateOneMeterQuad()
    {
        Mesh Return = new Mesh();

        Return.Vertices = new Vertex[]
        {
            new Vertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)), // Bottom-left
            new Vertex(new Vector3( 0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)), // Bottom-right
            new Vertex(new Vector3( -0.5f,  -0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)), // Top-right
            new Vertex(new Vector3(-0.5f,  0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f)), // Top-left
        };

        Return.Indices = new uint[]
        {
          0,1,3,
          1,2,3  
        };
        
        return Return;
    }

    public static Mesh GenerateOneMeterCube()
    {
        Mesh Return = new Mesh();

        Return.Vertices = new Vertex[]
        {
            // Front face
                new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(0.0f, 1.0f)),

                // Back face
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0.0f, 1.0f)),

                // Left face
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitX, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), -Vector3.UnitX, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitX, new Vector2(0.0f, 1.0f)),

                // Right face
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f), Vector3.UnitX, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), Vector3.UnitX, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), Vector3.UnitX, new Vector2(0.0f, 1.0f)),

                // Top face
                new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, 0.5f, -0.5f), Vector3.UnitY, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, 0.5f,  0.5f), Vector3.UnitY, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-0.5f, 0.5f,  0.5f), Vector3.UnitY, new Vector2(0.0f, 1.0f)),

                // Bottom face
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(0.0f, 1.0f)),
        };
        Return.Indices = new uint[]
        {
             // Front face
                0, 1, 2, 2, 3, 0,
                // Back face
                4, 5, 6, 6, 7, 4,
                // Left face
                8, 9, 10, 10, 11, 8,
                // Right face
                12, 13, 14, 14, 15, 12,
                // Top face
                16, 17, 18, 18, 19, 16,
                // Bottom face
                20, 21, 22, 22, 23, 20
        };

        return Return;
    }

    public static Mesh GenerateSphere(float radius = 0.5f, int sectorCount = 16, int stackCount = 16)
    {
        List<Vertex> _vertices = new List<Vertex>();
        List<uint> _indices = new List<uint>();
          float x, y, z, xy;
            float sectorStep = 2.0f * MathF.PI / sectorCount;
            float stackStep = MathF.PI / stackCount;
            float sectorAngle, stackAngle;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = MathF.PI / 2 - i * stackStep;
                xy = radius * MathF.Cos(stackAngle);
                z = radius * MathF.Sin(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    x = xy * MathF.Cos(sectorAngle);
                    y = xy * MathF.Sin(sectorAngle);

                    _vertices.Add(new Vertex(
                        new Vector3(x, y, z),
                        new Vector3(x / radius, y / radius, z / radius), // Normal
                        new Vector2((float)j / sectorCount, (float)i / stackCount) // Texture Coordinate
                    ));
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }

                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        Mesh Return = new Mesh()
        {
            Vertices = _vertices.ToArray(),
            Indices = _indices.ToArray()
        };

        return Return;
    }
}