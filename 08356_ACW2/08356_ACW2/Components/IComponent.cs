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
        COMPONENT_PHYSICSCOLLSION = 1 << 7,
        COMPONENT_WORLDCOLLSION = 1 << 8,
        COMPONENT_CYLINDERCOLLSION = 1 << 9,
        COMPONENT_DOOMCOLLSION = 1 << 10,

    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
