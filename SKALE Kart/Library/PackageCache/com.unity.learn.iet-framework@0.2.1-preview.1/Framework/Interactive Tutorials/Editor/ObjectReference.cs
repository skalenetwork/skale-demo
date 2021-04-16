using System;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    public class ObjectReference
    {
        [SerializeField]
        SceneObjectReference m_SceneObjectReference;

        [SerializeField]
        FutureObjectReference m_FutureObjectReference = null;

        public bool future { get { return m_FutureObjectReference != null; } }

        public SceneObjectReference sceneObjectReference
        {
            get
            {
                if (future)
                    return m_FutureObjectReference.sceneObjectReference;
                return m_SceneObjectReference ?? (m_SceneObjectReference = new SceneObjectReference());
            }
            set
            {
                if (!future)
                    m_SceneObjectReference = value;
            }
        }
    }
}
