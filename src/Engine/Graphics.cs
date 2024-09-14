using OpenTK.Mathematics;

namespace VoxelGame.Engine;

public static class Graphics
{
    public static Camera ActiveCamera;
    public static Matrix4 ShadowMatrix => 
    Matrix4.CreateOrthographicOffCenter(-35.0f, 35.0f, -35.0f, 35.0f, 0.1f, 75.0f);
    public static Matrix4 ShadowViewMatrix => Matrix4.LookAt(new Vector3(-30,20,-10),new Vector3(0,0,0),Vector3.UnitY);
}