﻿using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentTransform : IComponent
    {
        private Vector3 m_Position;
        private Vector3 m_Scale;
        private Vector3 m_Rotation;
        private Vector3 m_OldPosition;
       
        public ComponentTransform(Vector3 pos, Vector3 scale, Vector3 rot)
        {
            m_Position = pos;
            m_Scale = scale;
            m_Rotation = rot;
        }

        public Vector3 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
        public Vector3 Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }
        public Vector3 Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }
        public Vector3 OldPosition
        {
            get { return m_OldPosition; }
            set { m_OldPosition = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TRANSFORM; }
        }
    }
}
