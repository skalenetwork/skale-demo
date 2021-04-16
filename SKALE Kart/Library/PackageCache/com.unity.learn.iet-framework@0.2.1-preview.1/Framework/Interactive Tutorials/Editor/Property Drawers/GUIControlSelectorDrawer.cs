using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [CustomPropertyDrawer(typeof(GUIControlSelector))]
    public class GUIControlSelectorDrawer : PropertyDrawer
    {
        private const string k_SelectorModePath = "m_SelectorMode";
        private const string k_GUIContentPath = "m_GUIContent";
        private const string k_ControlNamePath = "m_ControlName";
        private const string k_PropertyPathPath = "m_PropertyPath";
        private const string k_TargetTypePath = "m_TargetType";
        private const string k_GUIStyleNamePath = "m_GUIStyleName";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var selectorMode = property.FindPropertyRelative(k_SelectorModePath);
            var height = EditorGUI.GetPropertyHeight(selectorMode);
            switch ((GUIControlSelector.Mode)selectorMode.intValue)
            {
                case GUIControlSelector.Mode.GUIContent:
                    height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_GUIContentPath), true);
                    break;
                case GUIControlSelector.Mode.NamedControl:
                    height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_ControlNamePath), true);
                    break;
                case GUIControlSelector.Mode.GUIStyleName:
                    height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_GUIStyleNamePath), true);
                    break;
                case GUIControlSelector.Mode.Property:
                    height +=
                        EditorGUIUtility.standardVerticalSpacing +
                        EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_TargetTypePath), true) +
                        EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_PropertyPathPath), true);
                    break;
                default:
                    height += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
                    break;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selectorMode = property.FindPropertyRelative(k_SelectorModePath);
            position.height = EditorGUI.GetPropertyHeight(selectorMode);
            EditorGUI.PropertyField(position, selectorMode);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            SerializedProperty selectorData = null;
            switch ((GUIControlSelector.Mode)selectorMode.intValue)
            {
                case GUIControlSelector.Mode.GUIContent:
                    selectorData = property.FindPropertyRelative(k_GUIContentPath);
                    break;
                case GUIControlSelector.Mode.NamedControl:
                    selectorData = property.FindPropertyRelative(k_ControlNamePath);
                    break;
                case GUIControlSelector.Mode.Property:
                    var targetType = property.FindPropertyRelative(k_TargetTypePath);
                    position.height = EditorGUI.GetPropertyHeight(targetType);
                    EditorGUI.PropertyField(position, targetType);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                    selectorData = property.FindPropertyRelative(k_PropertyPathPath);
                    break;
                case GUIControlSelector.Mode.GUIStyleName:
                    selectorData = property.FindPropertyRelative(k_GUIStyleNamePath);
                    break;
            }
            if (selectorData != null)
            {
                position.height = EditorGUI.GetPropertyHeight(selectorData, true);
                EditorGUI.PropertyField(position, selectorData, true);
            }
            else
            {
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.HelpBox(
                    position,
                    string.Format("No drawing implemented yet for selector mode {0}", (GUIControlSelector.Mode)selectorMode.intValue),
                    MessageType.Error
                    );
            }
        }
    }
}
