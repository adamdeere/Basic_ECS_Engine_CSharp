using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Pacman_Remaster;

public class Game : GameWindow
{
    public Game() : base(GameWindowSettings.Default,
                         new NativeWindowSettings()
                         {
                             Size = new Vector2i(1280, 720),
                             Title = "OpenTK Starter",
                         })
    {
    }

    int _vao;
    int _vbo;
    int _shader;

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color4.Black);

        float[] vertices = {
         // X     Y     Z
          0.0f, 0.5f, 0.0f,
         -0.5f,-0.5f, 0.0f,
          0.5f,-0.5f, 0.0f,
    };

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                      vertices, BufferUsageHint.StaticDraw);

        // Compile shaders
        const string vertexShaderSrc = @"
    #version 330 core
    layout(location = 0) in vec3 aPosition;
    void main()
    {
        gl_Position = vec4(aPosition, 1.0);
    }";

        const string fragmentShaderSrc = @"
    #version 330 core
    out vec4 FragColor;
    void main()
    {
        FragColor = vec4(1.0, 0.3, 0.2, 1.0);
    }";

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSrc);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSrc);
        GL.CompileShader(fragmentShader);

        _shader = GL.CreateProgram();
        GL.AttachShader(_shader, vertexShader);
        GL.AttachShader(_shader, fragmentShader);
        GL.LinkProgram(_shader);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        // Describe the vertex layout
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_shader);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }
}

