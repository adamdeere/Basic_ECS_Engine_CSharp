using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    enum ComponentTypes {
        COMPONENT_NONE = 0,
	    COMPONENT_TRANSFORM = 1 << 0,
        COMPONENT_MODEL = 1 << 1,
        COMPONENT_MATERIAL  = 1 << 2
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
