using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.InteractiveTutorials;
using Object = UnityEngine.Object;


[Serializable]
public class SceneObjectReference
{
    [SerializeField]
    private string m_SceneGuid;
    [SerializeField]
    private string m_GameObjectGuid;
    [SerializeField]
    private SerializedType m_SerializedComponentType = new SerializedType(null);
    [SerializeField]
    private int m_ComponentIndex;
    [SerializeField]
    private Object m_AssetObject;
    [SerializeField]
    private GameObject m_Prefab;

    [NonSerialized]
    private bool m_Initialized;
    [NonSerialized]
    private Object m_ReferencedObject;

    private SerializedProperty m_SceneGuidProperty;
    private SerializedProperty m_GameObjectGuidProperty;
    private SerializedProperty m_ComponentTypeProperty;
    private SerializedProperty m_SerializedComponentTypeProperty;
    private SerializedProperty m_ComponentIndexProperty;
    private SerializedProperty m_AssetObjectProperty;
    private SerializedProperty m_PrefabProperty;

    public Object ReferencedObject
    {
        get
        {
            if (!m_Initialized)
            {
                Init();
            }
            return m_ReferencedObject;
        }
    }

    public GameObject ReferencedObjectAsGameObject
    {
        get { return ReferencedObject as GameObject; }
    }

    public Component ReferencedObjectAsComponent
    {
        get { return ReferencedObject as Component; }
    }

    public bool IsGameObjectReference
    {
        get { return !string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.type == null && m_AssetObject == null; }
    }

    public bool IsComponentReference
    {
        get { return !string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.type != null && m_AssetObject == null; }
    }

    public bool IsAssetReference
    {
        get { return string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.type == null && m_AssetObject != null; }
    }

    public bool IsPrefabReference
    {
        get { return string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab != null && m_SerializedComponentType.type == null && m_AssetObject == null; }
    }

    public bool ReferenceResolved
    {
        get
        {
            return m_AssetObject != null || ReferencedObject != null || string.IsNullOrEmpty(m_GameObjectGuid);
        }
    }

    public SceneAsset ReferenceScene
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(m_SceneGuid));
        }
    }

    public SceneObjectReference()
    {
    }

    public SceneObjectReference(SerializedProperty property)
    {
        m_SceneGuidProperty = property.FindPropertyRelative("m_SceneGuid");
        m_GameObjectGuidProperty = property.FindPropertyRelative("m_GameObjectGuid");
        m_SerializedComponentTypeProperty = property.FindPropertyRelative("m_SerializedComponentType.m_TypeName");
        m_ComponentIndexProperty = property.FindPropertyRelative("m_ComponentIndex");
        m_AssetObjectProperty = property.FindPropertyRelative("m_AssetObject");
        m_PrefabProperty = property.FindPropertyRelative("m_Prefab");

        m_SceneGuid = m_SceneGuidProperty.stringValue;
        m_GameObjectGuid = m_GameObjectGuidProperty.stringValue;
        m_SerializedComponentType = new SerializedType(Type.GetType(m_SerializedComponentTypeProperty.stringValue));
        m_ComponentIndex = m_ComponentIndexProperty.intValue;
        m_AssetObject = m_AssetObjectProperty.objectReferenceValue;
        m_Prefab = m_PrefabProperty.objectReferenceValue as GameObject;

        Init();
    }

    private void Init()
    {
        m_Initialized = true;
        m_ReferencedObject = null;

        EditorSceneManager.sceneOpened += OnSceneOpened;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (m_AssetObject != null)
        {
            m_ReferencedObject = m_AssetObject;
            return;
        }

        if (m_Prefab != null)
        {
            m_ReferencedObject = m_Prefab;
            return;
        }

        if (string.IsNullOrEmpty(m_GameObjectGuid))
        {
            return;
        }

        var guidComponent = SceneObjectGUIDManager.instance.GetComponent(m_GameObjectGuid);
        if (guidComponent == null)
        {
            return;
        }
        GameObject go;
        m_ReferencedObject = go = guidComponent.gameObject;
        if (m_SerializedComponentType.type == null)
            return;
        var componentType = m_SerializedComponentType.type;
        if (componentType == null)
            return;
        var components = go.GetComponents(componentType);

        if (components.Length == 0)
        {
            Debug.LogWarning("Component " + componentType + " not found.");
            ResetReference();
            SaveProperties();
            return;
        }

        if (m_ComponentIndex + 1 > components.Length)
        {
            Debug.LogWarning("Component with given index " + m_ComponentIndex + " did not exist");
            m_ComponentIndexProperty.intValue = m_ComponentIndex = 0;
        }
        m_ReferencedObject = components[m_ComponentIndex];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetInitialization();
    }

    private void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        ResetInitialization();
    }

    private void OnSceneClosed(Scene scene)
    {
        ResetInitialization();
    }

    public void Update(UnityEngine.Object newObject)
    {
        ResetInitialization();
        ResetReference();
        if (newObject == null)
        {
            SaveProperties();
            return;
        }

        SceneObjectGUIDComponent guidComponent = null;
        Component component = null;
        GameObject go = null;

        if (newObject is Component)
        {
            component = newObject as Component;
            go = component.gameObject;
            guidComponent = go.GetComponent<SceneObjectGUIDComponent>();
            m_SerializedComponentType = new SerializedType(component.GetType());
            m_ComponentIndex = Array.IndexOf(go.GetComponents(component.GetType()), component);
        }
        else if (newObject is GameObject)
        {
            go = newObject as GameObject;
            if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                m_Prefab = go;
                SaveProperties();
                return;
            }
            guidComponent = go.GetComponent<SceneObjectGUIDComponent>();
        }
        else
        {
            m_ReferencedObject = m_AssetObject = newObject;
            SaveProperties();
            return;
        }

        if (guidComponent == null)
        {
            guidComponent = go.AddComponent<SceneObjectGUIDComponent>();
            Undo.RegisterCreatedObjectUndo(guidComponent, "Created GUID component");
        }

        m_GameObjectGuid = guidComponent.id;
        m_SceneGuid = GetSceneId(go);
        if (string.IsNullOrEmpty(m_SceneGuid))
        {
            Debug.LogError("The scene needs to be saved");
            return;
        }

        SaveProperties();
    }

    private void ResetReference()
    {
        m_SceneGuid = m_GameObjectGuid = null;
        m_SerializedComponentType = new SerializedType(null);
        m_ComponentIndex = 0;
        m_AssetObject = null;
        m_ReferencedObject = null;
        m_Prefab = null;
    }

    private void ResetInitialization()
    {
        m_Initialized = false;
        EditorSceneManager.sceneOpened -= OnSceneOpened;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SaveProperties()
    {
        if (m_GameObjectGuidProperty == null)
        {
            return;
        }
        m_GameObjectGuidProperty.stringValue = m_GameObjectGuid;
        m_SceneGuidProperty.stringValue = m_SceneGuid;
        m_SerializedComponentTypeProperty.stringValue = m_SerializedComponentType.type == null ? "" : m_SerializedComponentType.type.AssemblyQualifiedName;
        m_ComponentIndexProperty.intValue = m_ComponentIndex;
        m_AssetObjectProperty.objectReferenceValue = m_AssetObject;
        m_PrefabProperty.objectReferenceValue = m_Prefab;
    }

    string GetSceneId(GameObject gameObject)
    {
        var scenePath = gameObject.scene.path;
        return UnityEditor.AssetDatabase.AssetPathToGUID(scenePath);
    }
}
