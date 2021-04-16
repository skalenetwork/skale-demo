using System;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    public class SerializedType : ISerializationCallbackReceiver
    {
        [SerializeField]
        string m_TypeName;

        public Type type
        {
            get { return string.IsNullOrEmpty(m_TypeName) ? null : Type.GetType(m_TypeName); }
            set { m_TypeName = value == null ? "" : value.AssemblyQualifiedName; }
        }

        public SerializedType(Type type)
        {
            this.type = type;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            // Remove "-testable" suffix from assembly names
            if (!ProjectMode.IsAuthoringMode() && m_TypeName != null)
            {
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-Editor-firstpass-testable", "Assembly-CSharp-Editor-firstpass");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-Editor-testable", "Assembly-CSharp-Editor");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-firstpass-testable", "Assembly-CSharp-firstpass");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-testable", "Assembly-CSharp");
            }
        }
    }
}
