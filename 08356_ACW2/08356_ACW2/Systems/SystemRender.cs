using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_MODEL | ComponentTypes.COMPONENT_MATERIAL);

        ShaderObject k;

        public SystemRender(ShaderObject h)
        {
            k = h;
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
                    return component.ComponentType == ComponentTypes.COMPONENT_MODEL;
                });
                Geometry[] geometry = ((ComponentModel)geometryComponent).Geometry();

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
            GL.UseProgram(k.GetProgramId);
          //  GL.CullFace(CullFaceMode.Front);
            GL.Uniform1(k.GetUniformStex, 0);
            mat.SetActiveTextues();

            Matrix4 worldViewProjection = world * MyGame.gameInstance.view * MyGame.gameInstance.projection;
            GL.UniformMatrix4(k.GetuniformMView, false, ref worldViewProjection);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        public void OnDelete()
        {
           
        }
    }
}
