using Pacman_Remaster.Objects;

namespace Pacman_Remaster.Managers
{
    class ShaderManager
    {
        List<ShaderObject> m_shaderList;
        public ShaderManager(string fileName)
        {
            m_shaderList = new List<ShaderObject>();
            using (StreamReader textureSR = new StreamReader(fileName))
            {
                while (textureSR.Peek() > -1)
                {

                    string line = textureSR.ReadLine();
                    string[] result = line.Split(["\n", "\r\n", ","], StringSplitOptions.RemoveEmptyEntries);
                    m_shaderList.Add(new ShaderObject(result[0], result[1], result[2]));

                }
            }
        }
        public ShaderObject FindShader(string tag)
        {
            for (int i = 0; i < m_shaderList.Count; i++)
            {
                if (m_shaderList[i].GetTag == tag)
                {
                    return m_shaderList[i];
                }
            }
            throw new ApplicationException("Could not find shader");
        }
        public void DeleteShaders()
        {
            foreach (var item in m_shaderList)
            {
                item.DeleteShader();
            }
        }
    }
}
