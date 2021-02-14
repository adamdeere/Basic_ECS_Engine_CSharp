using Assimp;
using System.Collections.Generic;

namespace OpenGL_Game.Objects
{
    class ModelObject
    {
        private string m_Tag;
        private Geometry[] m_Geometry;
        public ModelObject(string tag, string fileName)
        {
            m_Tag = tag;
            Scene model;
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66.0f));
            model = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);
            m_Geometry = new Geometry[model.MeshCount];
            int count = 0;
            foreach (var item in model.Meshes)
            {
                List<Vector3D[]> vertList = new List<Vector3D[]>
                    {
                        item.Vertices.ToArray(),
                        item.TextureCoordinateChannels[0].ToArray(),
                        item.Normals.ToArray(),
                        item.BiTangents.ToArray(),
                        item.Tangents.ToArray()
                    };
                m_Geometry[count] = new Geometry(vertList, item.GetIndices(), item.VertexCount);
                count++;

            }
        }

        public string GetModelTag
        {
            get { return m_Tag; }
        }

        public Geometry[] GetGeometry
        {
            get { return m_Geometry; }
        }
    }
}
