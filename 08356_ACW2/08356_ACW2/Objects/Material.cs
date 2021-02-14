using System.Collections.Generic;

namespace OpenGL_Game.Objects
{
    class Material
    {
        private List<TextureObject> m_TextureList;
        private string m_MatTag;
        public Material(string tag)
        {
            m_TextureList = new List<TextureObject>();
            m_MatTag = tag;
        }

        public string GetMatTag
        {
            get { return m_MatTag; }
        }


        public void AddTexture(TextureObject tex)
        {
            m_TextureList.Add(tex);
        }
    }
}
