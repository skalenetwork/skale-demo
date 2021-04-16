using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    public class GUIControlSelector
    {
        public enum Mode
        {
            GUIContent,
            NamedControl,
            Property,
            GUIStyleName,
        }

        public Mode selectorMode { get { return m_SelectorMode; } set { m_SelectorMode = value; } }
        [SerializeField]
        private Mode m_SelectorMode;

        public GUIContent guiContent { get { return new GUIContent(m_GUIContent); } set { m_GUIContent = new GUIContent(value); } }
        [SerializeField]
        private GUIContent m_GUIContent = new GUIContent();

        public string controlName { get { return m_ControlName; } set { m_ControlName = value ?? ""; } }
        [SerializeField]
        private string m_ControlName = "";

        public string propertyPath { get { return m_PropertyPath; } set { m_PropertyPath = value ?? ""; } }
        [SerializeField]
        private string m_PropertyPath = "";

        public Type targetType { get { return m_TargetType.type; } set { m_TargetType.type = value; } }
        [SerializeField, SerializedTypeFilter(typeof(UnityObject))]
        private SerializedType m_TargetType = new SerializedType(null);

        public string guiStyleName { get { return m_GUIStyleName; } set { m_GUIStyleName = value; } }
        [SerializeField]
        private string m_GUIStyleName;
    }
}
