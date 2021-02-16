using System.Collections.Generic;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System;

namespace OpenGL_Game.Systems
{
    class SystemCollsion : ISystem
    {
        private List<Entity> m_EntityList;
        public SystemCollsion(List<Entity> list)
        {
            m_EntityList = list;
        }
        private const ComponentTypes MASK = (ComponentTypes.COMPONENT_COLLSION | ComponentTypes.COMPONENT_TRANSFORM);
        public string Name => "system collsion";

        public void OnAction(Entity entity, float dt)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent CylinderTransfrom = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                ComponentTransform transform = (ComponentTransform)CylinderTransfrom;

                IComponent collsionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLSION;
                });
                ComponentCollsion collsion = (ComponentCollsion)collsionComponent;

                UpdateCylinder(collsion, transform.ModelMatrix);
            }
        }
        private void UpdateCylinder(ComponentCollsion collsionComp, Matrix4 cylinderMatrix)
        {
            for (int i = 0; i < m_EntityList.Count; i++)
            {
                List<IComponent> components = m_EntityList[i].Components;
                IComponent CylinderCollision = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_CYLINDERCOLLSION;
                });
                if (CylinderCollision != null)
                {
                    IComponent SphereTransfrom = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                    });
                    ComponentTransform sphere = (ComponentTransform)SphereTransfrom;
                    Vector3 spherePosition = sphere.Position;

                    IComponent physicsComp = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_PHYSICS;
                    });
                    ComponentPhysics spherePhys = (ComponentPhysics)physicsComp;

                  

                    Vector3 circleInCollsionSpace = Vector3.TransformPosition(spherePosition, cylinderMatrix.Inverted());
                    Vector3 LNormal = (collsionComp.LinePointOne - collsionComp.LinePointTwo).Normalized();

                    Vector3 A = Vector3.Dot(circleInCollsionSpace - collsionComp.LinePointTwo, LNormal) * LNormal;
                    float LowerBoundsA = Vector3.Dot(circleInCollsionSpace - collsionComp.LinePointTwo, LNormal);
                    //red line = ltwo + A - pos
                    Vector3 redLine = (collsionComp.LinePointTwo + A) - circleInCollsionSpace;
                    //if redline < radius of circle
                    if (redLine.Length < (spherePhys.Radius))
                    {
                        if (A.Length > collsionComp.LineDistance.Length || LowerBoundsA < 0)
                        {
                            //do nothing
                        }
                        else
                        {
                            spherePhys.Velocity = Vector3.Transform(spherePhys.Velocity, cylinderMatrix.ExtractRotation().Inverted());
                            Vector3 rad = new Vector3(spherePhys.Radius, spherePhys.Radius, spherePhys.Radius);
                            Vector3 normal = Vector3.Transform((circleInCollsionSpace - redLine).Normalized(), cylinderMatrix.ExtractRotation().Inverted());
                            //sphere.Position = sphere.OldPosition + rad * normal;

                            spherePhys.Velocity -= 2 * Vector3.Dot(normal, spherePhys.Velocity) * normal;
                            spherePhys.Velocity = Vector3.Transform(spherePhys.Velocity, cylinderMatrix.ExtractRotation());
                            float distance = Math.Abs(Vector3.Dot(circleInCollsionSpace, normal));
                            Vector3 direction = spherePhys.Velocity.Normalized();
                        }

                    }
                }
            }
        }


    }
}
