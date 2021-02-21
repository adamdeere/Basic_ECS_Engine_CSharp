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

        ShaderObject m_PBR_Shader;

        public SystemRender(ShaderObject inShader)
        {
            m_PBR_Shader = inShader;
        }


        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity, float dt)
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

        public void Draw(Matrix4 model, Geometry geometry, ComponentMaterial mat)
        {
            GL.UseProgram(m_PBR_Shader.GetProgramId);
         
            GL.Uniform1(m_PBR_Shader.GetUniformColourTex, 0);
            GL.Uniform1(m_PBR_Shader.GetUniformHeightTex, 1);
            GL.Uniform1(m_PBR_Shader.GetUniformMetalicTex, 2);
            GL.Uniform1(m_PBR_Shader.GetUniformNormalTex, 3);
            GL.Uniform1(m_PBR_Shader.GetUniformRoughnessTex, 4);

            mat.SetActiveTextues();

            Matrix4 worldViewProjection = model * MyGame.gameInstance.view * MyGame.gameInstance.projection;

            Vector3 eyePos = MyGame.gameInstance.view.ExtractTranslation();
            Vector4 h = new Vector4(eyePos.X, eyePos.Y, eyePos.Z, 1);


            GL.Uniform4(m_PBR_Shader.Get_EyePosition, h);
            GL.Uniform4(m_PBR_Shader.Get_Lightposiytion, MyGame.gameInstance.lightPosition);

            GL.UniformMatrix4(m_PBR_Shader.GetuniforMVP_Matrix, false, ref worldViewProjection);
            GL.UniformMatrix4(m_PBR_Shader.Get_uniform_ModelMatrix, false, ref model);
            GL.UniformMatrix4(m_PBR_Shader.Get_uniform_ViewMatrix, false, ref MyGame.gameInstance.view);
            GL.UniformMatrix4(m_PBR_Shader.Get_uniform_ProjectionMatrix, false, ref MyGame.gameInstance.projection);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
