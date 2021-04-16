using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [CustomPropertyDrawer(typeof(MaskingSettings))]
    public class MaskingSettingsDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, ReorderableList> m_UnmaskedViewsPerPropertyPath =
            new Dictionary<string, ReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabled = property.FindPropertyRelative(MaskingSettings.k_EnabledPath);
            var height = EditorGUI.GetPropertyHeight(enabled);
            if (enabled.boolValue)
                height += EditorGUIUtility.standardVerticalSpacing + GetListControl(property).GetHeight();
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enabled = property.FindPropertyRelative(MaskingSettings.k_EnabledPath);
            position.height = EditorGUI.GetPropertyHeight(enabled);
            using (new EditorGUI.PropertyScope(position, label, enabled))
                property.isExpanded = enabled.boolValue = EditorGUI.ToggleLeft(position, label, enabled.boolValue);

            if (!property.isExpanded)
                return;

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            var listControl = GetListControl(property);
            position.height = listControl.GetHeight();
            using (new EditorGUI.IndentLevelScope())
                position = EditorGUI.IndentedRect(position);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                listControl.DoList(position);
        }

        private ReorderableList GetListControl(SerializedProperty parentProperty)
        {
            string key = parentProperty.propertyPath;
            ReorderableList list;
            if (!m_UnmaskedViewsPerPropertyPath.TryGetValue(key, out list))
            {
                list = new ReorderableList(parentProperty.serializedObject, parentProperty.FindPropertyRelative(MaskingSettings.k_UnmaskedViewsPath));
                list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Unmasked Views");
                list.elementHeightCallback = index =>
                    EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index), true) +
                    EditorGUIUtility.standardVerticalSpacing;
                list.drawElementCallback = (rect, index, isActive, isFocused) =>
                    EditorGUI.PropertyField(rect, list.serializedProperty.GetArrayElementAtIndex(index), true);
                m_UnmaskedViewsPerPropertyPath[key] = list;
            }
            return list;
        }
    }
}
