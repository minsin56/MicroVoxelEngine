using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public class BasicRenderPass : RenderPass
{
    public override void Init()
    {
    }

    public override void Render(Shader Shader, List<IRenderable> Renderables)
    {
        Shader.SetVec3("CamPos",Graphics.ActiveCamera.Position);
        foreach(var Renderable in Renderables)
        {
            Renderable.Render(Shader);
        }
    }
}