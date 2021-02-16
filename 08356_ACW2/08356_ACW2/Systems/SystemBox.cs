using System.Collections.Generic;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System;

namespace OpenGL_Game.Systems
{
    class SystemBox : ISystem
    {
        List<Entity> m_EntityList; 
        public SystemBox(List<Entity> list)
        {
            m_EntityList = list;
        }
        private const ComponentTypes MASK = (ComponentTypes.COMPONENT_BOX_COLLSION | ComponentTypes.COMPONENT_TRANSFORM);
        public string Name => "system box";

        public void OnAction(Entity entity, float dt)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                //gets the transform of the tower to extract the transform so I can do maths in tower space
                IComponent transfromComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                });
                ComponentTransform transform = (ComponentTransform)transfromComponent;

                Vector3 position = transform.Position;
                Vector3 rotation = transform.Rotation;
                Vector3 scale = transform.Scale;

                Matrix4 modelMatrix = Matrix4.CreateScale(scale) *
                                Matrix4.CreateRotationX(rotation.X) *
                                Matrix4.CreateRotationY(rotation.Y) *
                                Matrix4.CreateRotationZ(rotation.Z) *
                                Matrix4.CreateTranslation(position);

                UpdateBoxCollsion(modelMatrix);
            }
        }
        public void UpdateBoxCollsion(Matrix4 modelMatrix)
        {
            //loops through each entity again, looking for a certain component to make sure calculations do not happen
            //on game objects that do not need it
            foreach (var item in m_EntityList)
            {
                List<IComponent> components = item.Components;

                IComponent worldCollsion = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_WORLDCOLLSION;
                });

                if (worldCollsion != null)
                {
                    //this is the spheres transform
                    IComponent transfromComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_TRANSFORM;
                    });
                    ComponentTransform transform = (ComponentTransform)transfromComponent;

                    //this is the spheres transform
                    IComponent physicsComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_PHYSICS;
                    });
                    ComponentPhysics physics = (ComponentPhysics)physicsComponent;
                   
                   // Vector3 sphereinTowerSpace = Vector3.Transform(transform.Position, modelMatrix.Inverted());
                    float mCircleRadius = transform.Scale.X;

                    if (transform.Position.Y  < -3.0)
                    {
                        //change this to teleport
                        transform.Position = new Vector3(-1, 4.2f, 0);
                        physics.Velocity = new Vector3(2, -1, 0);
                    }
                    else if (transform.Position.Y > 4.5)
                    {
                        Vector3 normal = Vector3.Transform(new Vector3(0, 1, 0), modelMatrix.ExtractRotation());
                        ChangeDirection(physics, transform, normal);
                    }

                    else if (transform.Position.Z < -1.2)
                    {
                        Vector3 normal = Vector3.Transform(new Vector3(0, 0, -1), modelMatrix.ExtractRotation());
                        ChangeDirection(physics, transform, normal);
                    }
                    else if (transform.Position.Z > 1.2)
                    {
                        Vector3 normal = Vector3.Transform(new Vector3(0, 0, 1), modelMatrix.ExtractRotation());
                        ChangeDirection(physics, transform, normal);
                    }


                    else if (transform.Position.X < -1.0)
                    {
                        Vector3 normal = Vector3.Transform(new Vector3(1, 0, 0), modelMatrix.ExtractRotation());
                        ChangeDirection(physics, transform, normal);
                    }
                    else if (transform.Position.X > 1.0)
                    {
                        Vector3 normal = Vector3.Transform(new Vector3(-1, 0, 0), modelMatrix.ExtractRotation());
                        ChangeDirection(physics, transform, normal);
                    }
                }
            }
        }

        public void ChangeDirection(ComponentPhysics physics, ComponentTransform transform, Vector3 normal)
        {
            transform.Position = transform.OldPosition;
            physics.Velocity -= 2 * Vector3.Dot(normal, physics.Velocity) * normal;
        }

        public void OnDelete()
        {
            throw new NotImplementedException();
        }
    }
}
