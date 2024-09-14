using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxelGame.Engine;
using VoxelGame.Voxel;

namespace VoxelGame.Game;


public class CubeRenderable : IRenderable
{
    private Mesh Mesh;

    public CubeRenderable()
    {
        Mesh = Mesh.GenerateOneMeterCube();
        Mesh.GenerateBuffers();
    }
    public void Render(Shader Shader)
    {
        Shader.SetMatrix("View", Graphics.ActiveCamera.GetViewMatrix());
        Shader.SetMatrix("Projection", Graphics.ActiveCamera.GetProjectionMatrix());
        Shader.SetMatrix("Transform",Matrix4.CreateTranslation(0,0,0));
        Mesh.Draw();
    }
}
class GameScene : Scene
{
    float Time;
    Texture2D Tex;
 
    public Shader MainShader;

    public Mesh Quad;
    World World;


    CubeRenderable Cube;

    public BasicRenderPass MainRenderPass;
    public ShadowRenderPass ShadowPass;

    public GameScene()
    {
    }

    public override void Load()
    {
        World = new World();
        MainShader = new Shader("Shaders/Main.vert", "Shaders/Main.frag");


        Cube = new CubeRenderable();
        MainRenderPass = new BasicRenderPass();
        ShadowPass = new ShadowRenderPass(8192,8192);

        OwningWindow.Resize += (e) => 
        {
            GL.Viewport(0,0,e.Width,e.Height);
        };

    }

    public override void Render(float DeltaTime)
    {
        GL.Disable(EnableCap.CullFace);
        //GL.BindFramebuffer(FramebufferTarget.Framebuffer,ShadowBuffer.Handle);
        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.DepthBufferBit);
        GL.ClearColor(0.5f,0.3f,0.3f,1);

        ShadowPass.Render(ShadowPass.FBOShader,new List<IRenderable>(){World});
        GL.Viewport(0,0,OwningWindow.Size.X,OwningWindow.Size.Y);
        MainShader.Use();
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D,ShadowPass.ShadowBuffer.DepthTexture);

        MainShader.SetInt("DepthMap",1);
        MainShader.SetMatrix("ShadowProjection",Graphics.ShadowMatrix);
        MainShader.SetMatrix("ShadowView", Graphics.ShadowViewMatrix);

        MainRenderPass.Render(MainShader,new List<IRenderable>(){World});



    }

    public override void Unload()
    {

    }
    public override void Update(float DeltaTime)
    {
        Time += DeltaTime;
        World.Update(DeltaTime);
    }
}
