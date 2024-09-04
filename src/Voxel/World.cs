using OpenTK.Mathematics;
using VoxelGame.Engine;

namespace VoxelGame.Voxel;

public class World
{
    public List<Chunk> ActiveChunks = new List<Chunk>();
    public Queue<Chunk> ChunksToUpdate = new Queue<Chunk>();

    public bool DoneUpdatingChunk = true;

    public float WorldTime;
    ChunkMesher Mesher = new ChunkMesher();
    FastNoiseLite Noise = new FastNoiseLite();


    public World()
    {
        Graphics.ActiveCamera = new Camera(new OpenTK.Mathematics.Vector3(0,0,-10));
        Noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        

        GenerateChunk(new Vector3(0,0,0));
            
          
    }

    public Chunk GenerateChunk(Vector3 Center)
    {
        Chunk Chunk = new Chunk(Center,Noise,Mesher,this);
        ActiveChunks.Add(Chunk);
        ChunksToUpdate.Enqueue(Chunk);
        return Chunk;
    }

    public void Draw()
    {
        foreach(var Chunk in ActiveChunks)
        {
            Chunk.Render();
        }
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
}