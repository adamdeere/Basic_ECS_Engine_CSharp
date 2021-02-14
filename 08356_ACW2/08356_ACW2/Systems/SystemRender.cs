using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_MATERIAL);

        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int attribute_vtex;
        protected int attribute_vpos;

        protected int attribute_vNorm;
        protected int attribute_vBiTan;
        protected int attribute_vTan;
       
        protected int uniform_stex;
        protected int uniform_mview;

        public SystemRender()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vtex = GL.GetAttribLocation(pgmID, "a_TexCoord");

            attribute_vNorm = GL.GetAttribLocation(pgmID, "a_Normal");
            attribute_vBiTan = GL.GetAttribLocation(pgmID, "a_biTan");
            attribute_vTan = GL.GetAttribLocation(pgmID, "a_Tan");
          

            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");

            uniform_stex  = GL.GetUniformLocation(pgmID, "s_texture");

            if (attribute_vpos == -1 || attribute_vtex == -1 || uniform_stex == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
            if (attribute_vNorm == -1 || attribute_vBiTan == -1 || attribute_vTan == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
        }

        void LoadShader(string filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry[] geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                ComponentTransform transform = ((ComponentTransform)positionComponent);
                Vector3 position = transform.Position;
                Vector3 rotation = transform.Rotation;
                Vector3 scale = transform.Scale;

                Matrix4 world = Matrix4.CreateScale(scale) *
                                Matrix4.CreateRotationX(rotation.X) *
                                Matrix4.CreateRotationY(rotation.Y) *
                                Matrix4.CreateRotationZ(rotation.Z) *
                                Matrix4.CreateTranslation(position);

              //  Matrix4 world = Matrix4.CreateTranslation(position);

                IComponent textureComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_MATERIAL;
                });
                ComponentMaterial mat = (ComponentMaterial)textureComponent;
             
                for (int i = 0; i < geometry.Length; i++)
                {
                     Draw(world, geometry[i], mat);
                }

            }
        }

        public void Draw(Matrix4 world, Geometry geometry, ComponentMaterial mat)
        {
            GL.UseProgram(pgmID);
            GL.CullFace(CullFaceMode.Front);
            GL.Uniform1(uniform_stex, 0);
            mat.SetActiveTextues();

            Matrix4 worldViewProjection = world * MyGame.gameInstance.view * MyGame.gameInstance.projection;
            GL.UniformMatrix4(uniform_mview, false, ref worldViewProjection);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
