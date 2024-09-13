using OpenTK.Graphics.OpenGL4;

namespace VoxelGame.Engine;

public class FrameBuffer
{
    public int Handle;
    private int RBO;
    public int Texture,DepthTexture;

    public FrameBuffer(int Width,int Height)
    {
        Handle = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer,Handle);

        Texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D,Texture);
        GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,Width,Height,0,PixelFormat.Rgb,PixelType.Float,IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0,TextureTarget.Texture2D,Texture,0);

        DepthTexture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D,DepthTexture);
        GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.DepthComponent,Width,Height,0,PixelFormat.DepthComponent,PixelType.Float,IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthTexture, 0);


        GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);


    }
}