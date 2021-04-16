using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    class MaskingSettings
    {
        public const string k_EnabledPath = "m_MaskingEnabled";
        public const string k_UnmaskedViewsPath = "m_UnmaskedViews";

        public bool enabled { get { return m_MaskingEnabled; } set { m_MaskingEnabled = value; } }
        [SerializeField, FormerlySerializedAs("m_Enabled")]
        private bool m_MaskingEnabled;

        public IEnumerable<UnmaskedView> unmaskedViews { get { return m_UnmaskedViews; } }
        [SerializeField]
        private List<UnmaskedView> m_UnmaskedViews = new List<UnmaskedView>();

        public void SetUnmaskedViews(IEnumerable<UnmaskedView> unmaskedViews)
        {
            m_UnmaskedViews.Clear();
            if (unmaskedViews != null)
                m_UnmaskedViews.AddRange(unmaskedViews);
        }
    }
}
