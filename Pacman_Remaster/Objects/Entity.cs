using Pacman_Remaster.Components;
using System.Diagnostics;


namespace Pacman_Remaster.Objects;

class Entity
{
    string name;
    List<IComponent> componentList = [];
    ComponentTypes mask;

    public Entity(string name)
    {
        this.name = name;
    }

    /// <summary>Adds a single component</summary>
    public void AddComponent(IComponent component)
    {
        Debug.Assert(component != null, "Component cannot be null");

        componentList.Add(component);
        mask |= component.ComponentType;
    }

    public string Name
    {
        get { return name; }
    }

    public ComponentTypes Mask
    {
        get { return mask; }
    }

    public List<IComponent> Components
    {
        get { return componentList; }
    }
}
