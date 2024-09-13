using System.Diagnostics;
using OpenTK.Mathematics;
using VoxelGame.Engine;
using SharpNoise;
using SharpNoise.Modules;

namespace VoxelGame.Voxel;

public class Chunk
{
    public static int VoxelsPerChunk = 32;
    public Mesh ChunkMesh;
    public Vector3 Center;

    bool DoneMeshing = false;

    public VoxelOctree Octree;
    private FastNoiseLite Noise;
    ChunkMesher Mesher;

    public World World;

    public Chunk(Vector3 Center, FastNoiseLite Noise, ChunkMesher Mesher, World Owner)
    {
        this.Mesher = Mesher;
        this.Center = Center;
        this.Noise = Noise;
        Octree = new VoxelOctree(VoxelsPerChunk ,Vector3.Zero);
        World = Owner;
    }


    
    public void Generate()
    {

        for (float x = 0; x < VoxelsPerChunk; x++)
        {
            for (float y = 0; y < VoxelsPerChunk; y++)
            {
                for (float z = 0; z < VoxelsPerChunk; z++)
                {
                    Vector3 VoxelPos = new Vector3(x,y,z) * 0.1f;
                    Vector3 WorldVoxelPos = VoxelPos + (Center * 0.1f * VoxelsPerChunk);
                    float NoiseValue = (float)Noise.GetNoise(x + (Center.X * VoxelsPerChunk) ,0, z  + (Center.Z * VoxelsPerChunk)) * 32;
                    float ColorNoise = (float)Noise.GetNoise((x + (Center.X * 64)) * 512 ,(y + (Center.Y * 64)) * 512,(z + (Center.Z * 64)) * 512);
                    float WorldY = y + (Center.Y * VoxelsPerChunk);
                    if (WorldY + NoiseValue < 64)
                    {
                        Octree.SetVoxel(VoxelPos,
                        new Vector3(0, Math.Clamp(ColorNoise * 1.2f,0.1f,1), 0),0.1f,true);
                    }
   

                }

            }
        }


        RegenMesh();



    }

    public void RegenMesh()
    {
               Task.Run(() => 
        {ChunkMesh = Mesher.GenerateChunkMesh(this);}).ContinueWith((W)=> DoneMeshing = true);
    }

    private void ApplyMesh()
    {
        ChunkMesh.GenerateBuffers();
    }

    public void Update()
    {
        if (DoneMeshing)
        {
            ApplyMesh();
            DoneMeshing = false;
            World.DoneUpdatingChunk = true;
        }
    }

    public void Render()
    {
        World.MainShader.Use();
        World.MainShader.SetMatrix("Transform", Matrix4.CreateTranslation(Center * (VoxelsPerChunk * 0.1f - 0.1f)));
        World.MainShader.SetMatrix("View", Graphics.ActiveCamera.GetViewMatrix());
        World.MainShader.SetMatrix("Projection", Graphics.ActiveCamera.GetProjectionMatrix());

        ChunkMesh?.Draw();
    }
}