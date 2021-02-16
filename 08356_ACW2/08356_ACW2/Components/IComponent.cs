namespace OpenGL_Game.Components
{
    enum ComponentTypes {
        COMPONENT_NONE = 0,
	    COMPONENT_TRANSFORM = 1 << 0,
        COMPONENT_MODEL = 1 << 1,
        COMPONENT_MATERIAL  = 1 << 2,
        COMPONENT_BOX_COLLSION = 3,
        COMPONENT_PHYSICS = 1 << 4,
        COMPONENT_COLLSION = 1 << 5,
        COMPONENT_DOOMSPHERE = 1 << 6,

    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
