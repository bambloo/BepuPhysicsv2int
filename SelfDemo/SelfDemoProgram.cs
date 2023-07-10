using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SelfDemo;
using System;

public class SelfDemoProgram : GameWindow
{
    double[] vertices = {
         0.5f,  0.5f, 0.0f,  // top right
         0.5f, -0.5f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  // bottom left
        -0.5f,  0.5f, 0.0f   // top left
    };

    uint[] indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private int _elementArrayBuffer;

    private Shader _shader;
    public SelfDemoProgram() : base(GameWindowSettings.Default, new NativeWindowSettings())
    {
        //mNativeWindow = new NativeWindow()
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(double), vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(double), 0);
        GL.EnableVertexAttribArray(0);
        //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(double), 3 * sizeof(double));
        //GL.EnableVertexAttribArray(1);

        _elementArrayBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementArrayBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        //_shader.Use();
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.Color4(0, 0, 0, 1);
        //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementArrayBuffer);
        //GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        
        //Code goes here.

        //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //GL.DeleteBuffer(VertexBufferObject);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        var input = KeyboardState;
        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources.
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        GL.DeleteProgram(_shader.Handle);
        base.OnUnload();
    }

    private static void Main(string[] args)
    {
        using (SelfDemoProgram program = new SelfDemoProgram())
        {
            program.Run();
        }
    }
}