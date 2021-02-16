using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentPhysics : IComponent
    {
        private Vector3 m_Velocity;

        public ComponentPhysics(Vector3 vel)
        {
            m_Velocity = vel;
        }

        public Vector3 Velocity { get => m_Velocity; set => m_Velocity = value; }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_PHYSICS;
    }
}
