using System.Collections.Generic;

namespace Pacman_Remaster.Objects
{
    class MaterialObject
    {
        private List<TextureObject> m_TextureList;
        private string m_MatTag;
        public MaterialObject(string tag)
        {
            m_TextureList = new List<TextureObject>();
            m_MatTag = tag;
        }

        public List<TextureObject> GetTextureList => m_TextureList;
        public string GetMatTag => m_MatTag;


        public void AddTexture(TextureObject tex)
        {
            m_TextureList.Add(tex);
        }
    }
}
