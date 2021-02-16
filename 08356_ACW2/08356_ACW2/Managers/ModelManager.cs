using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Objects;
using System.IO;

namespace OpenGL_Game.Managers
{
    class ModelManager
    {
        private Dictionary<string, ModelObject> m_ModelDictionary = new Dictionary<string, ModelObject>();
      
        private int[] mVertexArrayObjectIDs;
        private int bufferCount = 0;
        public ModelManager(string fileName)
        {
            using (StreamReader modelSR = new StreamReader(fileName))
            {

                while (modelSR.Peek() > -1)
                {

                    string line = modelSR.ReadLine();
                    string[] result = line.Split(new string[] { "\n", "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);
                    m_ModelDictionary.Add(result[0], new ModelObject(result[0], "Geometry/" + result[1], result[2]));
                }
            }
            foreach (var item in m_ModelDictionary)
            {
                bufferCount += item.Value.GetNumberOfMeshes;
            }
        }
        public void BindGeometetry(ShaderManager sManager)
        {
            mVertexArrayObjectIDs = new int[bufferCount];
            GL.GenVertexArrays(mVertexArrayObjectIDs.Length, mVertexArrayObjectIDs);
            int loopCount = 0;
            foreach (var item in m_ModelDictionary)
            {
                Geometry[] geo = item.Value.GetGeometry;
                foreach (var mesh in geo)
                {
                    mesh.BindGeometry(loopCount, ref mVertexArrayObjectIDs, sManager.FindShader(item.Value.GetShaderType));
                    loopCount++;
                }
            }
        }
        public ModelObject FindModel(string tag)
        {
            foreach (var item in m_ModelDictionary)
            {
                if (item.Value.GetModelTag == tag)
                {
                    return item.Value;
                }
            }
            throw new ArgumentException("could not find model");
        }

        public  void DeleteBuffers()
        {
            GL.DeleteVertexArrays(bufferCount, mVertexArrayObjectIDs);
            foreach (var mesh in m_ModelDictionary)
            {
                mesh.Value.DeleteBuffere();
            }
        }
    }
}
