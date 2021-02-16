using System;
using System.Collections.Generic;
using System.Text;

namespace JSON_Parser
{
    class ModelData
    {
        public string name;
        public string fileName;
        public string mat;
        public ModelData(string nameT, string matName)
        {
            name = nameT;
            fileName = name+".fbx";
            mat = matName;
        }
    }
}
