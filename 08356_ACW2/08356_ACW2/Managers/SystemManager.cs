using System.Collections.Generic;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    class SystemManager
    {
        List<ISystem> systemList = new List<ISystem>();
        private TimerObject m_Timer;
      
        public SystemManager()
        {
            m_Timer = new TimerObject();
        }

        public void ActionSystems(EntityManager entityManager)
        {
            float deltaTime = m_Timer.GetElapsedSeconds();
            List<Entity> entityList = entityManager.Entities();
            foreach(ISystem system in systemList)
            {
                foreach(Entity entity in entityList)
                {
                    system.OnAction(entity, deltaTime);
                }
            }
        }
        public void StartTimer()
        {
            m_Timer.StartTimer();
        }
        public void AddSystem(ISystem system)
        {
            ISystem result = FindSystem(system.Name);
            //Debug.Assert(result != null, "System '" + system.Name + "' already exists");
            systemList.Add(system);
        }

        private ISystem FindSystem(string name)
        {
            return systemList.Find(delegate(ISystem system)
            {
                return system.Name == name;
            }
            );
        }
        public void DeleteShaders()
        {
            foreach (ISystem system in systemList)
            {
                system.OnDelete();
            }
        }
    }
}
