using System.Collections.Generic;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System;

namespace OpenGL_Game.Systems
{
    class SystemDoomSphere : ISystem
    {
        private List<Entity> m_EntityList;
        public SystemDoomSphere(List<Entity> list)
        {
            m_EntityList = list;
        }
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_DOOMSPHERE);
        public string Name => "system doom sphere";

        public void OnAction(Entity entity, float dt)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent DoomSphereTransfrom = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                ComponentTransform transform = (ComponentTransform)DoomSphereTransfrom;
                Vector3 position = transform.Position;
                float radius = transform.Radius;

                UpdateDoomSphere(position, radius);
            }
        }

        /// <summary>
        /// this method checks everything that can collide with a doom sphere and checks to see if it has
        /// if the radius happens to be zero, it remove the ball from the list
        /// </summary>
        /// <param name="doomPosition"></param>
        /// <param name="doomSphereRadius"></param>
        public void UpdateDoomSphere(Vector3 doomPosition, float doomSphereRadius)
        {
            for (int i = 0; i < m_EntityList.Count; i++)
            {
                List<IComponent> components = m_EntityList[i].Components;
                IComponent doomCollision = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DOOMCOLLSION;
                });
                if (doomCollision != null)
                {
                    IComponent sphereTransfrom = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                    });
                    ComponentTransform collidedWithPosition = (ComponentTransform)sphereTransfrom;
                    IComponent physicsTransform = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_PHYSICS;
                    });

                    ComponentPhysics collidedWithPhysics = (ComponentPhysics)physicsTransform;
                    float radius = collidedWithPhysics.Radius;

                    if (radius <= 0)
                    {
                        m_EntityList.RemoveAt(i);
                    }
                    else if ((doomPosition - collidedWithPosition.Position).Length < (doomSphereRadius + radius))
                    {
                        Vector3 ballScale = collidedWithPosition.Scale;
                        Vector3 scale = new Vector3(ballScale.X - .1f, ballScale.Y - .1f, ballScale.Y - .1f);
                        collidedWithPhysics.ResizeBall();
                        collidedWithPosition.Scale = scale;
                    }

                }
            }
           
        }
    }
}
