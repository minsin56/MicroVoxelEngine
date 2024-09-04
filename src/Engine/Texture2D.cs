using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace VoxelGame.Engine;

class Texture2D : Texture
{
    public override void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D,Handle);
    }

    public override void Load(string Path)
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D,Handle);

        using(Stream Stream = File.OpenRead(Path))
        {
            var Image = ImageResult.FromStream(Stream,ColorComponents.RedGreenBlueAlpha);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Image.Width, Image.Height, 0,
                              PixelFormat.Rgba, PixelType.UnsignedByte, Image.Data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D,0);
    }
}