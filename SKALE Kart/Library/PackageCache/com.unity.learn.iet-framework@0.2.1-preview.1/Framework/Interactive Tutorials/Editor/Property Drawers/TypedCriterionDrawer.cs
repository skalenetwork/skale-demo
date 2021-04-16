using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    [CustomPropertyDrawer(typeof(TypedCriterion))]
    class TypedCriterionDrawer : PropertyDrawer
    {
        // criterionProperty is a SerializedProperty on the SerializedObject for the Criterion
        delegate void PropertyIteratorCallback(SerializedProperty criterionProperty);

        const string k_TypeField = "type";
        const string k_CriterionField = "criterion";

        Dictionary<String, SerializedObject> m_PerPropertyCriterionSerializedObjects =
            new Dictionary<String, SerializedObject>();

        Rect m_CriterionPropertyRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.isExpanded = true;
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            IterateCriterion(
                property.FindPropertyRelative(k_CriterionField),
                p => height += EditorGUI.GetPropertyHeight(p) + EditorGUIUtility.standardVerticalSpacing
                );
            height -= EditorGUIUtility.standardVerticalSpacing;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            Rect typeFieldPosition = position;
            typeFieldPosition.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(typeFieldPosition, property.FindPropertyRelative(k_TypeField));
            if (EditorGUI.EndChangeCheck())
                OnCriterionTypeChanged(property);

            position.y += typeFieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
            position.height -= typeFieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
            m_CriterionPropertyRect = position;
            IterateCriterion(property.FindPropertyRelative(k_CriterionField), OnGUIIterateCriterion);
        }

        SerializedObject GetSerializedObject(SerializedProperty criterionProperty)
        {
            if (criterionProperty.objectReferenceValue == null)
                return null;

            string key = criterionProperty.propertyPath;
            SerializedObject serializedObject;
            var found = m_PerPropertyCriterionSerializedObjects.TryGetValue(key, out serializedObject);
            if (!found || serializedObject.targetObject == null)
            {
                serializedObject = new SerializedObject(criterionProperty.objectReferenceValue);
                m_PerPropertyCriterionSerializedObjects[key] = serializedObject;
            }

            return serializedObject;
        }

        void IterateCriterion(SerializedProperty criterion, PropertyIteratorCallback onIterateChildProperty)
        {
            if (criterion.objectReferenceValue == null)
                return;

            var serializedObject = GetSerializedObject(criterion);
            if (serializedObject == null)
                return;

            var childProperty = serializedObject.GetIterator();
            childProperty.NextVisible(true);
            while (childProperty.NextVisible(childProperty.isExpanded))
            {
                if (string.Equals(childProperty.propertyPath, "m_Script", StringComparison.Ordinal))
                    continue;

                onIterateChildProperty(childProperty);
            }
        }

        void OnGUIIterateCriterion(SerializedProperty criterionProperty)
        {
            criterionProperty.serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            m_CriterionPropertyRect.height = EditorGUI.GetPropertyHeight(criterionProperty);
            EditorGUI.PropertyField(m_CriterionPropertyRect, criterionProperty, true);
            m_CriterionPropertyRect.y += m_CriterionPropertyRect.height + EditorGUIUtility.standardVerticalSpacing;

            if (EditorGUI.EndChangeCheck())
            {
                criterionProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        void OnCriterionTypeChanged(SerializedProperty parentProperty)
        {
            var criterionProperty = parentProperty.FindPropertyRelative(k_CriterionField);

            if (criterionProperty.objectReferenceValue != null)
                Undo.DestroyObjectImmediate(criterionProperty.objectReferenceValue);

            var criterionType = System.Type.GetType(
                    parentProperty.FindPropertyRelative(k_TypeField).FindPropertyRelative("m_TypeName").stringValue
                    );

            if (criterionType != null)
            {
                var criterion = ScriptableObject.CreateInstance(criterionType);
                Undo.RegisterCreatedObjectUndo(criterion, "Change Criterion");
                criterion.hideFlags |= HideFlags.HideInHierarchy;

                AssetDatabase.AddObjectToAsset(criterion, parentProperty.serializedObject.targetObject);
                string parentAssetPath = AssetDatabase.GetAssetPath(parentProperty.serializedObject.targetObject);
                AssetDatabase.ImportAsset(parentAssetPath);

                criterionProperty.objectReferenceValue = criterion;

                m_PerPropertyCriterionSerializedObjects.Clear();
            }
            else
            {
                criterionProperty.objectReferenceValue = null;
            }
        }
    }
}
