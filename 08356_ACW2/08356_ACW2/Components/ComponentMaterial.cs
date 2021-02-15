using OpenGL_Game.Objects;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenGL_Game.Components
{
    class ComponentMaterial : IComponent
    {
        private MaterialObject m_ObjectMat;
        public ComponentMaterial(MaterialObject objectMat)
        {
            m_ObjectMat = objectMat;
        }

        public MaterialObject GetMat => m_ObjectMat;
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_MATERIAL;

        public void SetActiveTextues()
        {
            List<TextureObject> texList = m_ObjectMat.GetTextureList;
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texList[0].GetTextureNumber);
            
            GL.Enable(EnableCap.Texture2D);
        }
    }
}
