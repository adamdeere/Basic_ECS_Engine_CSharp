using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentModel : IComponent
    {
        private Geometry[] geometry;
        private ModelObject m_ModelObject;

        public ComponentModel(ModelObject geometryObject)
        {
            m_ModelObject = geometryObject;
            geometry = m_ModelObject.GetGeometry;
           
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_MODEL; }
        }

        public Geometry[] Geometry()
        {
            return geometry;
        }
    }
}
