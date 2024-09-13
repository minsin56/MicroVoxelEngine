using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public class Shader
{
    int Handle;

    public Shader(string VertexPath, string FragmentPath)
    {
        string VertexShaderSource = File.ReadAllText(VertexPath);
        string FragmentShaderSource = File.ReadAllText(FragmentPath);

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader,VertexShaderSource);

        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader,FragmentShaderSource);

        Handle = GL.CreateProgram();
        GL.CompileShader(VertexShader);

        GL.GetShader(VertexShader,ShaderParameter.CompileStatus, out int Success);

        if(Success == 0)
        {
            string InfoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(InfoLog);
        }

        GL.CompileShader(FragmentShader);
        
        GL.GetShader(FragmentShader,ShaderParameter.CompileStatus, out Success);

        if(Success == 0)
        {
            string InfoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(InfoLog);
        }

        GL.AttachShader(Handle,VertexShader);
        GL.AttachShader(Handle,FragmentShader);
        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }


    public int GetUniformLocation(string Name)
    {
        return GL.GetUniformLocation(Handle,Name);
    }

    public void SetInt(string Name, int Value)
    {
        GL.Uniform1(GetUniformLocation(Name),Value);
    }

    public void SetMatrix(string Name, Matrix4 Mat)
    {
        GL.UniformMatrix4(GetUniformLocation(Name),false,ref Mat);
    }
}