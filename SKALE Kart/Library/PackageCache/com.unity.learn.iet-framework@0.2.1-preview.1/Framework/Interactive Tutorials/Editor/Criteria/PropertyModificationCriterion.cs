using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class PropertyModificationCriterion : Criterion
    {
        internal enum TargetValueMode
        {
            TargetValue = 0,
            DifferentThanInitial
        }

        internal string propertyPath
        {
            get { return m_PropertyPath; }
            set { m_PropertyPath = value; }
        }
        [SerializeField]
        string m_PropertyPath;

        internal TargetValueMode targetValueMode
        {
            get { return m_TargetValueMode; }
            set { m_TargetValueMode = value; }
        }

        [SerializeField]
        TargetValueMode m_TargetValueMode = TargetValueMode.TargetValue;


        // TODO: Make this more like TypedCriterion
        internal string targetValue
        {
            get { return m_TargetValue; }
            set { m_TargetValue = value; }
        }
        [SerializeField]
        [Tooltip("This value only applies if the TargetValueMode is set to TargetValue. This field will have no effects in other modes.")]
        string m_TargetValue;

        internal TargetValueType targetValueType
        {
            get { return m_TargetValueType; }
            set { m_TargetValueType = value; }
        }
        [SerializeField]
        TargetValueType m_TargetValueType;

        internal SceneObjectReference target
        {
            get { return m_Target.sceneObjectReference; }
            set { m_Target.sceneObjectReference = value; }
        }
        [SerializeField]
        ObjectReference m_Target = new ObjectReference();

        [NonSerialized]
        string m_InitialValue;

        public override void StartTesting()
        {
            var target = m_Target.sceneObjectReference.ReferencedObject;
            if(m_TargetValueMode == TargetValueMode.TargetValue)
                completed = PropertyFulfillCriterion(target, m_PropertyPath);
            else
            {
                var so = new SerializedObject(target);
                var sp = so.FindProperty(propertyPath);

                if (sp == null)
                    Debug.LogWarningFormat("PropertyModificationCriterion: Cannot find property \"{0}\" on \"{1}\"", propertyPath, target);
                else
                    m_InitialValue = GetPropertyValueAsString(sp);
            }

            Undo.postprocessModifications += PostprocessModifications;
            Undo.undoRedoPerformed += UpdateCompletion;
        }

        public override void StopTesting()
        {
            Undo.postprocessModifications -= PostprocessModifications;
            Undo.undoRedoPerformed -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            var targetObject = m_Target.sceneObjectReference.ReferencedObject;
            return PropertyFulfillCriterion(targetObject, m_PropertyPath);
        }

        UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications)
        {
            var targetObject = m_Target.sceneObjectReference.ReferencedObject;
            var modificationsToTest = GetPropertiesToTest(modifications, targetObject);
            if (modificationsToTest.Any())
            {
                completed = modificationsToTest.Any(m => PropertyFulfillCriterion(m.target, m.propertyPath));
            }

            return modifications;
        }

        IEnumerable<PropertyModification> GetPropertiesToTest(UndoPropertyModification[] modifications, UnityObject target)
        {
            var result = new List<PropertyModification>();
            foreach (var m in modifications)
            {
                if (m.currentValue.target == target)
                { 
                    if(IsCompoundPropertyMatch(m.currentValue.propertyPath))
                    {
                        var propertyModification = m.currentValue;
                        propertyModification.propertyPath = propertyPath;
                        result.Add(m.currentValue);
                    }
                    else if(m.currentValue.propertyPath == m_PropertyPath)
                        result.Add(m.currentValue);
                }
            }
            return result;
        }

        bool IsCompoundPropertyMatch(string propertyPath)
        {
            if (m_TargetValueType == TargetValueType.Color)
            {
                Regex coloRegex = new Regex(m_PropertyPath + "\\.[rgba]");
                if (coloRegex.IsMatch(propertyPath))
                    return true;
            }
            return propertyPath == m_PropertyPath;
        }

        bool DoPropertyTypeMatches(SerializedProperty property)
        {
            switch (m_TargetValueType)
            {
                case TargetValueType.Decimal:
                    return property.propertyType == SerializedPropertyType.Float;
                case TargetValueType.Integer:
                    return property.propertyType == SerializedPropertyType.Integer;
                case TargetValueType.Text:
                    return property.propertyType == SerializedPropertyType.String;
                case TargetValueType.Boolean:
                    return property.propertyType == SerializedPropertyType.Boolean;
                case TargetValueType.Color:
                    return property.propertyType == SerializedPropertyType.Color;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new Exception("unknown TargetValueType");
        }

        string GetPropertyValueAsString(SerializedProperty property)
        {
            switch (targetValueType)
            {
                case TargetValueType.Decimal:
                    return property.floatValue.ToString();
                case TargetValueType.Integer:
                    return property.intValue.ToString();
                case TargetValueType.Text:
                    return property.stringValue;
                case TargetValueType.Boolean:
                    return property.boolValue.ToString();
                case TargetValueType.Color:
                    return property.colorValue.ToString();
            }

            throw new Exception("unknown TargetValueType");
        }

        bool DoesPropertyMatches(SerializedProperty property,string value)
        {
            switch (targetValueType)
            {
                case TargetValueType.Decimal:
                {
                    float convertedValue;
                    return float.TryParse(value, out convertedValue) &&
                        Mathf.Approximately(property.floatValue, convertedValue);
                }

                case TargetValueType.Integer:
                {
                    int convertedValue;
                    return int.TryParse(value, out convertedValue) && property.intValue == convertedValue;
                }
                case TargetValueType.Text:
                {
                    return property.stringValue == value;
                }
                case TargetValueType.Boolean:
                {
                    bool convertedValue;
                    return bool.TryParse(value, out convertedValue) && property.boolValue == convertedValue;
                }
                case TargetValueType.Color:
                {
                    Color convertedValue;
                    return ColorUtility.TryParseHtmlString(value, out convertedValue) && property.colorValue == convertedValue;
                }
            }

            return false;
        }

        bool SetPropertyTo(SerializedProperty property, string value)
        {
            switch (targetValueType)
            {
                case TargetValueType.Decimal:
                {
                    float convertedTargetValue;
                    if (!float.TryParse(value, out convertedTargetValue))
                        return false;

                    property.floatValue = convertedTargetValue;
                    return true;
                }
                case TargetValueType.Integer:
                {
                    int convertedTargetValue;
                    if (!int.TryParse(value, out convertedTargetValue))
                        return false;

                    property.intValue = convertedTargetValue;
                    return true;
                }
                case TargetValueType.Text:
                {
                    property.stringValue = value;
                    return true;
                }
                case TargetValueType.Boolean:
                {
                    bool convertedTargetValue;
                    if (!bool.TryParse(value, out convertedTargetValue))
                        return false;
                    property.boolValue = convertedTargetValue;
                    return true;
                }
                case TargetValueType.Color:
                {
                    Color convertedTargetValue;
                    if (!ColorUtility.TryParseHtmlString(value, out convertedTargetValue))
                        return false;
                    property.colorValue = convertedTargetValue;
                    return true;
                }
            }
            return false;
        }

        
        bool SetPropertyToDifferentValueThan(SerializedProperty property, string value)
        {
            switch (targetValueType)
            {
                case TargetValueType.Decimal:
                {
                    float convertedTargetValue;
                    if (!float.TryParse(value, out convertedTargetValue))
                        return false;

                    property.floatValue = convertedTargetValue + 1.0f;
                    return true;
                }
                case TargetValueType.Integer:
                {
                    int convertedTargetValue;
                    if (!int.TryParse(value, out convertedTargetValue))
                        return false;

                    property.intValue = convertedTargetValue + 1;
                    return true;
                }
                case TargetValueType.Text:
                {
                    property.stringValue = value + "different ";
                    return true;
                }
                case TargetValueType.Boolean:
                {
                    bool convertedTargetValue;
                    if (!bool.TryParse(value, out convertedTargetValue))
                        return false;
                    property.boolValue = !convertedTargetValue;
                    return true;
                }
                case TargetValueType.Color:
                {
                    Color convertedTargetValue;
                    if (!ColorUtility.TryParseHtmlString(value, out convertedTargetValue))
                        return false;
                    property.colorValue = convertedTargetValue + Color.gray;
                    return true;
                }
            }
            return false;
        }


        bool PropertyFulfillCriterion(UnityObject target, string propertyPath)
        {
            if (target == null)
                return false;

            if (m_TargetValueMode == TargetValueMode.TargetValue &&  m_TargetValueType != TargetValueType.Text && string.IsNullOrEmpty(m_TargetValue))
                return true;

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyPath);

            if (property == null)
                return false;
            
            if (!DoPropertyTypeMatches(property))
                return false;

            switch (m_TargetValueMode)
            {
                case TargetValueMode.TargetValue:
                    return DoesPropertyMatches(property, m_TargetValue);
                case TargetValueMode.DifferentThanInitial:
                    return !DoesPropertyMatches(property, m_InitialValue);
            }

            return false;
        }

        public override bool AutoComplete()
        {
            var target = m_Target.sceneObjectReference.ReferencedObject;
            if (target == null)
                return false;

            if (m_TargetValueMode == TargetValueMode.TargetValue && m_TargetValueType != TargetValueType.Text && string.IsNullOrEmpty(m_TargetValue))
                return false;

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(m_PropertyPath);

            if (property == null)
                return false;

            if (!DoPropertyTypeMatches(property))
                return false;

            switch (m_TargetValueMode)
            {
                case TargetValueMode.TargetValue:
                {
                    if (!SetPropertyTo(property, targetValue))
                        return false;
                    break;
                }
                case TargetValueMode.DifferentThanInitial:
                {
                    if (!SetPropertyToDifferentValueThan(property, m_InitialValue))
                        return false;
                    break;
                }
            }

            serializedObject.ApplyModifiedProperties();

            return true;
        }

        internal enum TargetValueType
        {
            Integer,
            Decimal,
            Text,
            Boolean,
            Color,
        }
    }
}
