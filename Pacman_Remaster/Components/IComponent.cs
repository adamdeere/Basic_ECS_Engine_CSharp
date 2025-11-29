namespace Pacman_Remaster.Components
{
    enum ComponentTypes {
        COMPONENT_NONE = 0,
	    COMPONENT_TRANSFORM = 1 << 0,
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
