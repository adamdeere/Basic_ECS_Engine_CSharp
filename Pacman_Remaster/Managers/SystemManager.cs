using Pacman_Remaster.Objects;
using Pacman_Remaster.Systems;

namespace Pacman_Remaster.Managers;

class SystemManager
{
    List<ISystem> systemList = [];
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
            for (int i = 0; i < entityList.Count; i++)
            {
                system.OnAction(entityList[i], deltaTime);
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
   
}
