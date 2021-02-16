using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Systems
{
    class SystemBox : ISystem
    {
        public string Name => "system box";

        public void OnAction(Entity entity, float dt)
        {
            throw new NotImplementedException();
        }

        public void OnDelete()
        {
            throw new NotImplementedException();
        }
    }
}
