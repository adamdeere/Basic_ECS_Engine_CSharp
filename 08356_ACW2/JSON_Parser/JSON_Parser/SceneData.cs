using System;

namespace JSON_Parser
{
    class SceneData
    {
        public string name;
        public string location;
        public string rotation;
        public string mat;

       public SceneData(string name, string loc, string rot, string mat)
        {
            string[] result = name.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            this.name = result[0];
            location = loc;
            rotation = rot;
            this.mat = mat;        
        }
    }
}
