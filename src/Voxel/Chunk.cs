using System.Diagnostics;
using System.Drawing;
using OpenTK.Mathematics;
using VoxelGame.Engine;
using SharpNoise;
using SharpNoise.Modules;

namespace VoxelGame.Voxel;

public class Chunk
{
    public Mesh ChunkMesh;
    public Vector3 Center;

    Shader MainShader;
    Texture Tex;

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
        Octree = new VoxelOctree(128,Center);
        World = Owner;
        MainShader = new Shader("Shaders/Main.vert", "Shaders/Main.frag");
        Tex = (Texture)new Texture2D();
        Tex.Load("wall.jpg");
    }

    public void Generate()
    {

        
        RidgedMulti Ridged = new RidgedMulti();
        Ridged.OctaveCount = 10;
        for (float x = 0; x < 128; x++)
        {
            for (float y = 0; y < 128; y++)
            {
                for (float z = 0; z < 128; z++)
                {

                    float NoiseValue = (float)Ridged.GetValue(x * 0.01f + Center.X,y  * 0.01f + Center.Y, z * 0.01f + Center.Z);
                    float ColorNoise = (float)Noise.GetNoise(x * 16,y * 16,z * 16);
                    
                    if (NoiseValue < 0.4f && IsInSphere(x,y,z, 128 / 2, 128 / 2, 128 / 2, 128 / 2))
                    {
                        Octree.SetVoxel(new Vector3(x * 0.1f, y * 0.1f, z * 0.1f),
                        new Vector3(0, Math.Clamp(ColorNoise * 1.2f,0.1f,1), 0),0.1f,true);
                    }
   

                }

            }
        }


        Task.Run(() => 
        {ChunkMesh = Mesher.GenerateChunkMesh(this);}).ContinueWith((W)=> DoneMeshing = true);



    }


    private bool IsInSphere(float x, float y, float z, float ox, float oy, float oz, float r)
    {
 x -= ox;
    y -= oy;
    z -= oz;
    return (x*x+y*y+z*z < r*r);
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
        MainShader.Use();
        MainShader.SetMatrix("Transform", Matrix4.CreateTranslation(Center * 12.0f));
        MainShader.SetMatrix("View", Graphics.ActiveCamera.GetViewMatrix());
        MainShader.SetMatrix("Projection", Graphics.ActiveCamera.GetProjectionMatrix());

        ChunkMesh?.Draw();
    }
}