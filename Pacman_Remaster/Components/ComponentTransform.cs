using OpenTK;
using OpenTK.Mathematics;

namespace Pacman_Remaster.Components;

class ComponentTransform : IComponent
{
    private Matrix4 mModelMatrix;
    private Vector3 m_Position;
    private Vector3 m_Scale;
    private Vector3 m_Rotation;
    private Vector3 m_OldPosition;
    private float m_Radius;
   
    public ComponentTransform(Vector3 pos, Vector3 scale, Vector3 rot, float radius)
    {
        m_Position = pos;
        m_Scale = scale;
        m_Rotation = rot;
        m_Radius = radius;

        mModelMatrix = Matrix4.CreateScale(scale.X, scale.Y, scale.Z);
        mModelMatrix *= Matrix4.CreateRotationX(m_Rotation.X);
        mModelMatrix *= Matrix4.CreateRotationY(m_Rotation.Y);
        mModelMatrix *= Matrix4.CreateRotationZ(m_Rotation.Z);
        mModelMatrix *= Matrix4.CreateTranslation(m_Position.X, m_Position.Y, m_Position.Z);
    }

    public Matrix4 ModelMatrix
    {
        get { return mModelMatrix; }
        set { mModelMatrix = value; }
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

    public float Radius
    {
        get { return m_Radius; }
        set { m_Radius = value; }
    }

    public ComponentTypes ComponentType
    {
        get { return ComponentTypes.COMPONENT_TRANSFORM; }
    }
}
