using System.Diagnostics;
using System.Drawing;
using Friflo.Engine.ECS;
using Friflo.Json.Fliox.Transform.Query.Ops;
using OpenTK.Mathematics;
using VoxelGame.Engine;

namespace VoxelGame.Voxel;

public class ChunkMesher
{


    public static Vertex[] FrontFaceVertices =
    [
        new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(-0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(0.0f, 1.0f)),
    ];

    public static Vertex[] BackFaceVertices =
    [
        new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0.0f, 1.0f)),
    ];

    public static Vertex[] LeftFaceVertices =
    [
        new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitX, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), -Vector3.UnitX, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitX, new Vector2(0.0f, 1.0f)),
    ];

    public static Vertex[] RightFaceVertices =
    [
        new Vertex(new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3(0.5f, -0.5f,  0.5f), Vector3.UnitX, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3(0.5f,  0.5f,  0.5f), Vector3.UnitX, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(0.5f,  0.5f, -0.5f), Vector3.UnitX, new Vector2(0.0f, 1.0f)),
    ];

    public static Vertex[] TopFaceVertices =
    [
        new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, 0.5f, -0.5f), Vector3.UnitY, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, 0.5f,  0.5f), Vector3.UnitY, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(-0.5f, 0.5f,  0.5f), Vector3.UnitY, new Vector2(0.0f, 1.0f)),
    ];


    public static Vertex[] BottomFaceVertices =
    [
        new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(0.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(1.0f, 0.0f)),
        new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(1.0f, 1.0f)),
        new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(0.0f, 1.0f)),
    ];


    public uint[] FaceIndices = [0, 1, 2, 2, 3, 0,];

    public Mesh GenerateChunkMesh(Chunk Chunk)
    {
        Mesh Mesh = new Mesh();

        List<Vertex> Vertices = new List<Vertex>(); 
        List<uint> Indices = new List<uint>();
        AddVoxel(Chunk.Octree.root, Vector3.Zero, 32, Chunk, Vertices, Indices);


        Mesh.Vertices = Vertices.ToArray();
        Mesh.Indices = Indices.ToArray();

        return Mesh;
    }


    public void AddVoxel(OctreeNode Node, Vector3 Origin, float Size, Chunk Chunk, List<Vertex> Vertices, List<uint> Indices)
    {
        if (Node == null || Node.IsEmpty())
        {
            return;
        }
        if (Node.IsLeaf && Node.HasVoxel)
        {

            Vector3[] CubeVertices =
            [
                new Vector3(Origin.X - Size / 2, Origin.Y - Size  /2 ,Origin.Z - Size / 2), // 0
                new Vector3(Origin.X + Size / 2, Origin.Y - Size  /2 ,Origin.Z - Size / 2), // 1
                new Vector3(Origin.X + Size / 2, Origin.Y + Size  /2 ,Origin.Z - Size / 2), // 2
                new Vector3(Origin.X - Size / 2, Origin.Y + Size  /2 ,Origin.Z - Size / 2), // 3
                new Vector3(Origin.X - Size / 2, Origin.Y - Size  /2 ,Origin.Z + Size / 2), // 4
                new Vector3(Origin.X + Size / 2, Origin.Y - Size  /2 ,Origin.Z + Size / 2), // 5
                new Vector3(Origin.X + Size / 2, Origin.Y + Size  /2 ,Origin.Z + Size / 2), // 6
                new Vector3(Origin.X - Size / 2, Origin.Y + Size  /2 , Origin.Z +  Size / 2)  // 7
            ];

             Vertex[] CubeVertex =  new Vertex[CubeVertices.Length];

            for (int i = 0; i < CubeVertices.Length; i++)
            {
                CubeVertex[i].Position = CubeVertices[i];
                CubeVertex[i].TexCoord = new Vector2(0, 0);
                CubeVertex[i].Normal = Node.Color;
            }

            Vector3[] Directions =
            [
                new Vector3(-1,  0,  0), // Left face
                new Vector3( 1,  0,  0), // Right face
                new Vector3( 0, -1,  0), // Bottom face
                new Vector3( 0,  1,  0), // Top face
                new Vector3( 0,  0, -1), // Front face
                new Vector3( 0,  0,  1)  // Back face
            ];

            uint[][] CubeFaceIndices =
            [
                new uint[] { 0, 3, 7, 4 }, // Left face
                new uint[] { 1, 5, 6, 2 }, // Right face
                new uint[] { 0, 1, 5, 4 }, // Bottom face
                new uint[] { 3, 2, 6, 7 }, // Top face
                new uint[] { 0, 1, 2, 3 }, // Front face
                new uint[] { 4, 5, 6, 7 }  // Back face
            ];


            for (int i = 0; i < 6; i++)
            {
                if(!Chunk.Octree.IsNeighborVoxel(Node,Directions[i] * Node.Size))
                {
                    AddFace(CubeVertex, CubeFaceIndices[i], Node.Position, Vertices, Indices, Size, Node);
                }
                
            }
        }
        if(Node.Children != null)
        {
            for(int i = 0; i < 8;i++)
            {
                if(Node.Children[i] != null)
                {
                    AddVoxel(Node.Children[i],Node.Children[i].Position,Node.Children[i].Size,Chunk,Vertices,Indices);
                }
            }
        }
    }

    public void AddFace(Vertex[] InVertices, uint[] InIndices, Vector3 Position, List<Vertex> Vertices, List<uint> Indices, float Size, OctreeNode Node)
    {
        uint Offset = (uint)Vertices.Count;

        foreach (int Index in InIndices)
        {
            Vertices.Add(InVertices[Index]);
        }

        Indices.Add(Offset);
        Indices.Add(Offset + 1);
        Indices.Add(Offset + 2);

        Indices.Add(Offset);
        Indices.Add(Offset + 2);
        Indices.Add(Offset + 3);

    }
}