using OpenTK;
using System;

namespace OpenGL_Game.Components
{
    class ComponentPhysics : IComponent
    {
        private Vector3 m_Velocity;
        private  float coeeficent;
        public float Radius { get; set; }
        public float Volume { get; set; }
        public float Density { get; set; }
        public float Mass { get; set; }

        public ComponentPhysics(Vector3 vel, float density, float radius)
        {
            Random r = new Random();
            float x = r.Next(-3, 4);
            float z = r.Next(-3, 3);
            Radius = radius;
            m_Velocity = new Vector3(x, 0, z); ;
            Density = density;
            Volume = (4 / 3) * (float)Math.PI * (float)Math.Pow(Radius, 3);
            Mass = density * Volume;
            coeeficent = .5f;
        }

        public Vector3 Velocity { get => m_Velocity; set => m_Velocity = value; }
        public float CoEfficent { get => coeeficent; }

        public void ResizeBall()
        {
            Radius -= 0.1f;
            Density -= 0.1f;
            Volume = (4 / 3) * (float)Math.PI * (float)Math.Pow(Radius, 3);
            Mass = Density * Volume;
        }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_PHYSICS;
    }
}
