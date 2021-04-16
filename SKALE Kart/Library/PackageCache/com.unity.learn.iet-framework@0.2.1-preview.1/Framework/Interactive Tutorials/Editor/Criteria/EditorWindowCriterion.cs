using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class EditorWindowCriterion : Criterion
    {
        [SerializedTypeFilter(typeof(EditorWindow))]
        [SerializeField]
        SerializedType m_EditorWindowType = new SerializedType(null);

        public SerializedType editorWindowType
        {
            get
            {
                return m_EditorWindowType;
            }
            set
            {
                m_EditorWindowType = value;
            }
        }

        [SerializeField]
        bool m_CloseIfAlreadyOpen;

        public bool closeIfAlreadyOpen
        {
            get
            {
                return m_CloseIfAlreadyOpen;
            }
            set
            {
                m_CloseIfAlreadyOpen = value;
            }
        }

        EditorWindow m_WindowInstance;

        public override void StartTesting()
        {
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        public override void StopTesting()
        {
            EditorApplication.update -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            if (m_EditorWindowType.type == null)
            {
                return false;
            }
            if (!m_WindowInstance)
            {
                var windows = Resources.FindObjectsOfTypeAll(m_EditorWindowType.type);

                foreach (var w in windows)
                {
                    if (w.GetType() == m_EditorWindowType.type)
                    {
                        m_WindowInstance = (EditorWindow)w;

                        m_WindowInstance.Focus();
                        return true;
                    }
                }
                return false;
            }
            if (m_WindowInstance.GetType() != m_EditorWindowType.type)
            {
                m_WindowInstance = null;
            }
            return true;
        }

        public override bool AutoComplete()
        {
            if(m_EditorWindowType.type == null)
            {
                return false;
            }

            var window = EditorWindow.GetWindow(m_EditorWindowType.type);
            return true;
        }
    }
}
