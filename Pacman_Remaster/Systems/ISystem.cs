using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pacman_Remaster.Objects;

namespace Pacman_Remaster.Systems
{
    interface ISystem
    {
        void OnAction(Entity entity, float dt);
       

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
