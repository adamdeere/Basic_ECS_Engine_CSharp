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
        private List<Entity> m_EntityList;
        public SystemPhysics(List<Entity> list)
        {
            m_EntityList = list;
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
                UpdatePhysicsCollsion(entity, transform, physics);
            }
        }

        public Vector3 UpdatePhysicsPosition(ComponentTransform transform, ComponentPhysics physics, float dt)
        {
            Vector3 position = transform.Position;
            transform.OldPosition = position;

            Vector3 velocity = physics.Velocity;

            return position += (velocity) * dt;
        }

        public void UpdatePhysicsCollsion(Entity self, ComponentTransform selfPosition, ComponentPhysics selfPhysics)
        {
            for (int i = 0; i < m_EntityList.Count; i++)
            {
                List<IComponent> components = m_EntityList[i].Components;
                IComponent physCollsion = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_PHYSICSCOLLSION;
                });
                if (physCollsion != null && m_EntityList[i] != self)
                {
                    IComponent sphereTransfrom = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                    });
                    ComponentTransform collidedWithPosition = (ComponentTransform)sphereTransfrom;

                    IComponent physicsComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_PHYSICS;
                    });

                    ComponentPhysics collidedwithPhysics = (ComponentPhysics)physicsComponent;

                    if ((selfPosition.Position - collidedWithPosition.Position).Length < (selfPhysics.Radius + collidedwithPhysics.Radius))
                    {
                        selfPosition.Position = selfPosition.OldPosition;

                        Vector3 normal = (collidedWithPosition.Position - selfPosition.Position).Normalized();

                        Vector3 selfVelocityCopy = selfPhysics.Velocity;
                        Vector3 collidedwithVelocityCopy = collidedwithPhysics.Velocity;

                        selfPhysics.Velocity -= Vector3.Dot(selfPhysics.Velocity, normal) * normal;
                        //velocity two
                        collidedwithPhysics.Velocity -= Vector3.Dot(collidedwithPhysics.Velocity, -normal) * -normal;

                        //velocity of this instance of the sphere
                        //𝑣1 = 𝑚1 * 𝑢1 + 𝑚2 * 𝑢2 + 𝑒 * 𝑚2(𝑢2 − 𝑢1) / 𝑚1 + 𝑚2
                        Vector3 first = selfPhysics.Mass * selfVelocityCopy;
                        Vector3 second = collidedwithPhysics.Mass * collidedwithVelocityCopy;

                        Vector3 co = collidedwithPhysics.CoEfficent * collidedwithPhysics.Mass * (collidedwithVelocityCopy - selfVelocityCopy);
                        Vector3 final = (first + second + co) / (selfPhysics.Mass + collidedwithPhysics.Mass);
                        selfPhysics.Velocity = final;

                        //velocity of the second instance of the sphere
                        //𝑣2 = (𝑚2 − 𝑚1 / 𝑚2 + 𝑚1) * 𝑢2 + (2𝑚1 𝑚2 + 𝑚1) * 𝑢1
                        float firstPart = (collidedwithPhysics.Mass - selfPhysics.Mass) / (collidedwithPhysics.Mass + selfPhysics.Mass);
                        float secondPart = (selfPhysics.Mass * 2) / (collidedwithPhysics.Mass + selfPhysics.Mass);
                        collidedwithPhysics.Velocity = (firstPart * collidedwithVelocityCopy) + (secondPart * selfVelocityCopy);
                    }
                }
            }
        }
    }
}
