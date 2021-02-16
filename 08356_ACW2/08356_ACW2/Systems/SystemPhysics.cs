using System.Collections.Generic;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;


namespace OpenGL_Game.Systems
{
    class SystemPhysics : ISystem
    {
        private const ComponentTypes MASK = (ComponentTypes.COMPONENT_PHYSICS | ComponentTypes.COMPONENT_TRANSFORM);
      
        private Vector3 m_gravity;

        public SystemPhysics()
        {
            m_gravity = new Vector3(0, -9.81f, 0);
        }
        public string Name => "system physics";
        public void OnAction(Entity entity, float dt)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

               
                IComponent transfromComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                ComponentTransform transform = (ComponentTransform)transfromComponent;

                IComponent physicsComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_PHYSICS;
                });

                ComponentPhysics physics = (ComponentPhysics)physicsComponent;

                transform.Position = UpdatePhysicsPosition(transform, physics, dt);
            }
        }

        public Vector3 UpdatePhysicsPosition(ComponentTransform transform, ComponentPhysics physics, float dt)
        {
            Vector3 position = transform.Position;
            transform.OldPosition = position;

            Vector3 velocity = physics.Velocity;

            return position += (velocity) * dt;
        }
    }
}
