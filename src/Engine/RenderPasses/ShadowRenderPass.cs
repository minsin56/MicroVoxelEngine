using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public class ShadowRenderPass : RenderPass
{
    public FrameBuffer ShadowBuffer;
    public Shader FBOShader;

    private Shader DebugShader;

    int Size;

    public Mesh Quad;

    public ShadowRenderPass(int ResX,int ResY)
    {
        ShadowBuffer = new FrameBuffer(ResX,ResY);
         Quad=  new Mesh();
        Quad.Vertices = [
            new Vertex(new Vector3(-1,1,0.0f),Vector3.Zero,new Vector2(0,1)),
            new Vertex(new Vector3(1,1,0.0f),Vector3.Zero,new Vector2(1,1)),
            new Vertex(new Vector3(1,-1,0.0f),Vector3.Zero,new Vector2(1,0)),
            new Vertex(new Vector3(-1,-1,0.0f),Vector3.Zero,new Vector2(0,0)),
        ];
        Quad.Indices = [0,1,2,
                        2,3,0];
        Quad.GenerateBuffers();
        Size = ResX;

        FBOShader = new Shader("Shaders/Depth.vert","Shaders/Depth.frag");
        DebugShader = new Shader("Shaders/DebugQuad.vert","Shaders/DebugQuad.frag");

    }

    public override void Init()
    {
    }

    public override void Render(Shader Shader, List<IRenderable> Renderables)
    {
        FBOShader.Use();
        FBOShader.SetMatrix("ShadowProjection", Graphics.ShadowMatrix);
        FBOShader.SetMatrix("ShadowView", Graphics.ShadowViewMatrix);

        GL.Enable(EnableCap.DepthTest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer,ShadowBuffer.Handle);
        GL.Viewport(0,0,Size,Size);

        GL.Clear(ClearBufferMask.DepthBufferBit);

        foreach(var Renderable in Renderables)
        {
            Renderable.Render(FBOShader);
        }


        GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
        // GL.Viewport(0,0,1280,720);

        // GL.Clear(ClearBufferMask.ColorBufferBit  | ClearBufferMask.DepthBufferBit);

        // DebugShader.Use();
        // GL.Disable(EnableCap.DepthTest);
        // GL.BindTexture(TextureTarget.Texture2D,ShadowBuffer.DepthTexture);
        // Quad.Draw();
    }
}