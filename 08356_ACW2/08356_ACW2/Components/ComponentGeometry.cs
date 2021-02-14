using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentGeometry : IComponent
    {
        private Geometry[] geometry;
        private ModelObject m_ModelObject;

        public ComponentGeometry(ModelObject geometryObject)
        {
            m_ModelObject = geometryObject;
            geometry = m_ModelObject.GetGeometry;
           
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
