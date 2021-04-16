using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    class InlineIcon
    {
        [SerializeField]
        UnityObject m_Asset = null;
        [SerializeField]
        string m_IconContentName = null;
        [SerializeField]
        string m_IconStyleName = null;

        public Texture GetTexture()
        {
            if (m_Asset == null)
                return EditorGUIUtility.IconContent(m_IconContentName).image;

            // use the same logic as in ProjectBrowser; see ObjectListLocalGroup.cs and CachedFilteredHierarchy.cs
            var texture = AssetPreview.GetAssetPreview(m_Asset);
            if (texture == null)
            {
                texture = m_Asset is GameObject ?
                    AssetPreview.GetMiniThumbnail(m_Asset) :
                    AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(m_Asset)) as Texture2D;
            }
            return texture;
        }

        public GUIStyle GetStyle()
        {
            return m_Asset == null ? GUIStyle.none :
                EditorGUIUtility.GetBuiltinSkin(EditorGUIUtility.isProSkin ? EditorSkin.Scene : EditorSkin.Inspector).FindStyle(m_IconStyleName);
        }
    }

    [Serializable]
    class InlineIconCollection : CollectionWrapper<InlineIcon> {}
}
