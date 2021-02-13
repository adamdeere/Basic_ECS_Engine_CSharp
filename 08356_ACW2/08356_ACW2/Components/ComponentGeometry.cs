using Assimp;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Collections.Generic;

namespace OpenGL_Game.Components
{
    class ComponentGeometry : IComponent
    {
        Geometry[] geometry;

        public ComponentGeometry(string geometryName)
        {
            Scene model;
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66.0f));
            model = importer.ImportFile(geometryName, PostProcessPreset.TargetRealTimeMaximumQuality);
            geometry = new Geometry[model.MeshCount];
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
                geometry[count] = new Geometry(vertList, item.GetIndices(), item.VertexCount);
                count++;

            }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_GEOMETRY; }
        }

        public Geometry[] Geometry()
        {
            return geometry;
        }
    }
}
