using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxelGame.Engine;
using VoxelGame.Voxel;

namespace VoxelGame.Game;

class GameScene : Scene
{
    float Time;
    Texture2D Tex;
    public FrameBuffer ShadowBuffer;
    public Shader ShadowBufferShader;

    public Mesh Quad;
    World World;

    public GameScene()
    {
        
    }

    public override void Load()
    {
        World = new World();
        ShadowBufferShader = new Shader("Shaders/Shadows.vert","Shaders/Shadows.frag");
        ShadowBuffer = new FrameBuffer(1280,720);
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
    }

    public override void Render(float DeltaTime)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer,ShadowBuffer.Handle);
        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.ClearColor(0.5f,0.3f,0.3f,1);
        World.Draw();

        ShadowBufferShader.Use();

        GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Disable(EnableCap.DepthTest);
        GL.BindTexture(TextureTarget.Texture2D,ShadowBuffer.DepthTexture);
        Quad.Draw();

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
