using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("")]
public class SceneObjectGUIDComponent : MonoBehaviour
{
    [SerializeField] private string m_Id;
    [NonSerialized] private bool m_Registered;

    public string id
    {
        get { return m_Id; }
    }

    public void Awake()
    {
        if (!Application.isEditor)
            Destroy(this);
        if (string.IsNullOrEmpty(m_Id))
        {
            m_Id = Guid.NewGuid().ToString();
        }
        else
        {
            var components = UnityEngine.Object.FindObjectsOfType<SceneObjectGUIDComponent>();
            if (components.Any(c => c.m_Id == m_Id && c != this))
            {
                m_Id = Guid.NewGuid().ToString();
            }
        }
        hideFlags |= HideFlags.HideInInspector;
        Register();
    }

    private void Register()
    {
        if (m_Registered || !Application.isEditor)
            return;
        SceneObjectGUIDManager.instance.Register(this);
        m_Registered = true;
    }

    void OnValidate()
    {
        // Register in OnValidate becuase Awake in not called on domain reload in edit mode
        Register();
    }

    void OnDestroy()
    {
        if (SceneObjectGUIDManager.IsInitiated)
            SceneObjectGUIDManager.instance.Deregister(this);
    }
}
