using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class FutureObjectReference : ScriptableObject
    {
        [SerializeField]
        SceneObjectReferenceHolder m_ReferenceHolder;

        [SerializeField]
        Criterion m_Criterion;

        [SerializeField]
        string m_ReferenceName;

        SceneObjectReferenceHolder referenceHolder
        {
            get
            {
                if (m_ReferenceHolder == null)
                {
                    m_ReferenceHolder = CreateInstance<SceneObjectReferenceHolder>();
                    m_ReferenceHolder.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_ReferenceHolder;
            }
        }

        public SceneObjectReference sceneObjectReference
        {
            get
            {
                if (referenceHolder.sceneObjectReference == null)
                    referenceHolder.sceneObjectReference = new SceneObjectReference();

                return referenceHolder.sceneObjectReference;
            }
            set
            {
                referenceHolder.sceneObjectReference = value;
            }
        }

        public Criterion criterion { get { return m_Criterion; } set { m_Criterion = value; } }

        public string referenceName { get { return m_ReferenceName; } set { m_ReferenceName = value; } }

        void OnDestroy()
        {
            if (m_ReferenceHolder != null)
                DestroyImmediate(m_ReferenceHolder);
        }
    }

    public class SceneObjectReferenceHolder : ScriptableObject
    {
        [SerializeField]
        SceneObjectReference m_SceneObjectReference;

        public SceneObjectReference sceneObjectReference
        {
            get { return m_SceneObjectReference; }
            set { m_SceneObjectReference = value; }
        }
    }
}
