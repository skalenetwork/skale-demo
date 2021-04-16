using System.Collections.Generic;
using UnityEngine.Assertions;

public class SceneObjectGUIDManager
{
    private static SceneObjectGUIDManager m_Instance;
    private Dictionary<string, SceneObjectGUIDComponent> components = new Dictionary<string, SceneObjectGUIDComponent>();

    public static SceneObjectGUIDManager instance
    {
        get
        {
            if (!IsInitiated)
            {
                m_Instance = new SceneObjectGUIDManager();
            }
            return m_Instance;
        }

        /*internal*/ set { m_Instance = value; }
    }

    /*internal */ public SceneObjectGUIDManager()
    {
    }

    public static bool IsInitiated { get { return m_Instance != null; } }

    public void Register(SceneObjectGUIDComponent component)
    {
        Assert.IsFalse(string.IsNullOrEmpty(component.id));

        //Add will trow an exception if the id is already registered
        components.Add(component.id, component);
    }

    public bool Contains(string id)
    {
        return components.ContainsKey(id);
    }

    public void Deregister(SceneObjectGUIDComponent component)
    {
        components.Remove(component.id);
    }

    public SceneObjectGUIDComponent GetComponent(string id)
    {
        SceneObjectGUIDComponent value;
        if (components.TryGetValue(id, out value))
        {
            return value;
        }
        return null;
    }
}
