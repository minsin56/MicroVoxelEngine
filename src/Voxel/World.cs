using OpenTK.Mathematics;
using VoxelGame.Engine;

namespace VoxelGame.Voxel;

public class World: IRenderable
{
    public static World ActiveWorld;
    public List<Chunk> ActiveChunks = new List<Chunk>();
    public Queue<Chunk> ChunksToUpdate = new Queue<Chunk>();

    public bool DoneUpdatingChunk = true;

    public float WorldTime;
    ChunkMesher Mesher = new ChunkMesher();
    FastNoiseLite Noise = new FastNoiseLite();

    public Shader MainShader;


    public World()
    {

        ActiveWorld = this;
        MainShader = new Shader("Shaders/Main.vert", "Shaders/Main.frag");
        Graphics.ActiveCamera = new Camera(new OpenTK.Mathematics.Vector3(0,0,-10));
        Noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);

        for(int x = -5;x < 5; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                for(int z = -5;z < 5;z++)
                {
                    GenerateChunk(new Vector3(x,y,z));
                }
            }
        }            
          
    }

    public Chunk GenerateChunk(Vector3 Center)
    {
        Chunk Chunk = new Chunk(Center,Noise,Mesher,this);
        ActiveChunks.Add(Chunk);
        ChunksToUpdate.Enqueue(Chunk);
        return Chunk;
    }

    public void Update(float DeltaTime = 0)
    {
        WorldTime += DeltaTime;
        foreach(var C in ActiveChunks)
        {
            C.Update();
        }
        if(DoneUpdatingChunk)
        {
            ThreadPool.QueueUserWorkItem(Work => 
            {
                ChunksToUpdate.TryDequeue(out var Chunk);
                Chunk?.Generate();
                DoneUpdatingChunk = false;
            });
        }
    }

    public void Render(Shader Shader)
    {
       
        foreach(var Chunk in ActiveChunks)
        {
            Shader.SetMatrix("View", Graphics.ActiveCamera.GetViewMatrix());
            Shader.SetMatrix("Projection", Graphics.ActiveCamera.GetProjectionMatrix());
            Shader.SetMatrix("Transform", Matrix4.CreateTranslation(Chunk.Center * (Chunk.VoxelsPerChunk * 0.1f - 0.1f)));

            Chunk.ChunkMesh?.Draw();
        }
    }
}