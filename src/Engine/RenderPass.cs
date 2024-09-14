namespace VoxelGame.Engine;

public abstract class RenderPass()
{
    public string ModelMatrixName;
    public string ProjectionMatrixName;
    public string ViewMatrixName;



    public abstract void Init();
    public abstract void Render(Shader Shader, List<IRenderable> Renderables);

}