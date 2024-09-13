namespace VoxelGame.Engine;

using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
public class Camera
{
    public Vector3 Position;

    public Vector3 Forward = -Vector3.UnitZ;
    public Vector3 Up = Vector3.UnitY;
    public Vector3 Right = Vector3.UnitX;

    public float Pitch = 0.0f, Yaw = 90.0f, Roll = 0.0f;

    public float FOV = 90.0f;

    private float Aspect;


    public Camera(Vector3 Position)
    {
        this.Position = Position;
        UpdateCameraVectors();
    }   


    public Matrix4 GetViewMatrix() => Matrix4.LookAt(Position, Position + Forward, Up);
    public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV),Aspect,0.1f,5000.0f);

    public void UpdateCameraVectors()
    {
        Aspect = MainWindow.Instance.Size.X / (float)MainWindow.Instance.Size.Y;
        Forward.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
        Forward.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
        Forward.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
        Forward = Vector3.Normalize(Forward);

        // Also re-calculate the Right and Up vector
        Right = Vector3.Normalize(Vector3.Cross(Forward, Vector3.UnitY));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
        Up = Vector3.Normalize(Vector3.Cross(Right, Forward));
    }
}