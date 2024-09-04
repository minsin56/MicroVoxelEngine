using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace VoxelGame.Engine
{
    public class MainWindow : GameWindow
    {
        public Scene LoadedScene;
        public static MainWindow Instance;
        public MainWindow(int Width, int Height, Scene InitialScene) : base(GameWindowSettings.Default, new NativeWindowSettings{Size = (Width, Height), Title = "Main"})
        {
            LoadedScene = InitialScene;
            Instance = this;
        }


        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            LoadedScene.OwningWindow = this;

            LoadedScene.Load();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearColor(0.5f,0.3f,0.3f,1);
            LoadedScene.Update((float)args.Time);
            LoadedScene.Render((float)args.Time);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0,0,Size.X,Size.Y);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.E)
            {
                Graphics.ActiveCamera.Yaw += 15f;
            }
            
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q)
            {
                Graphics.ActiveCamera.Yaw -= 15f;
            }



            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift)
            {
                Graphics.ActiveCamera.Position.Y += 0.5f;
            }
            
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftControl)
            {
                Graphics.ActiveCamera.Position.Y -= 0.5f;
            }

            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.A)
            {
                Graphics.ActiveCamera.Position.X += 0.5f;
            }
            
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.D)
            {
                Graphics.ActiveCamera.Position.X -= 0.5f;
            }
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.W)
            {
                Graphics.ActiveCamera.Position += Graphics.ActiveCamera.Forward * 0.5f;
            }
            if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.S)
            {
                Graphics.ActiveCamera.Position -= Graphics.ActiveCamera.Forward * 0.5f;
            }
            Graphics.ActiveCamera.UpdateCameraVectors();
        }
    }
}