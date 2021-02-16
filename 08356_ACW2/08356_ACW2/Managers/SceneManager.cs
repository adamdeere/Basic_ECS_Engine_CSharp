using OpenTK;
using System;

namespace OpenGL_Game.Managers
{
    class SceneManager
    {
        private string name;
        private string position;
        private string rotation;
        private string mat;

        public string Mat { get => mat; set => mat = value; }

        public string Name { get => name; set => name = value; }
      
        public string Rotation { get => rotation; set => rotation = value; }
        public string Location { get => position; set => position = value; }

        public Vector3 ConvertToVector(string number)
        {
            string[] splitStrings = number.Split(',');

            float x = ((float)Math.Round(float.Parse(splitStrings[0]) * 100f) / 100f);
            float y = ((float)Math.Round(float.Parse(splitStrings[1]) * 100f) / 100f);
            float z = ((float)Math.Round(float.Parse(splitStrings[2]) * 100f) / 100f);
            return new Vector3(x,y,z);
        }
    }
}
