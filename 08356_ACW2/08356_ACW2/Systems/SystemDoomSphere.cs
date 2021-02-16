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

        public void UpdateDoomSphere(Vector3 doomPosition, float doomSphereRadius)
        {
            foreach (var item in m_EntityList)
            {
                List<IComponent> components = item.Components;
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

                    if ((doomPosition - collidedWithPosition.Position).Length < (doomSphereRadius + collidedWithPosition.Radius))
                    {
                       
                    }
                   
                }
            }
        }
    }
}
