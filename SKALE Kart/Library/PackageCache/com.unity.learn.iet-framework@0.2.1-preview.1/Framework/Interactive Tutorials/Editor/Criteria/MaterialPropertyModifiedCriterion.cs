using System;
using System.Transactions;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class MaterialPropertyModifiedCriterion : Criterion
    {
        internal SceneObjectReference target
        {
            get { return m_Target.sceneObjectReference; }
            set { m_Target.sceneObjectReference = value; }
        }
        [SerializeField]
        ObjectReference m_Target = new ObjectReference();

        internal string materialPropertyPath
        {
            get { return m_MaterialPropertyPath; }
            set { m_MaterialPropertyPath = value; }
        }
        [SerializeField]
        string m_MaterialPropertyPath = "";

        string m_InitialValue = null;

        static MaterialProperty FindProperty(string path, Material material)
        {
            UnityObject[] mats = new[] { material };
            return MaterialEditor.GetMaterialProperty(mats, path);
        }

        static string GetPropertyValueToString(MaterialProperty property)
        {
            switch (property.type)
            {
                case MaterialProperty.PropType.Color:
                    return property.colorValue.ToString();
                case MaterialProperty.PropType.Vector:
                    return property.vectorValue.ToString();
                case MaterialProperty.PropType.Float:
                    return property.floatValue.ToString();
                case MaterialProperty.PropType.Range:
                    return property.rangeLimits.ToString();
                case MaterialProperty.PropType.Texture:
                    return property.textureValue.GetInstanceID().ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void StartTesting()
        {
            InitializeRequiredStateIfNeeded();

            EditorApplication.update += UpdateCompletion;
        }

        void InitializeRequiredStateIfNeeded()
        {
            if (m_InitialValue != null)
                return;

            if(string.IsNullOrEmpty(m_MaterialPropertyPath) || target.ReferencedObject == null)
                return;

            var property = FindProperty(m_MaterialPropertyPath, (Material)target.ReferencedObject);

            m_InitialValue = GetPropertyValueToString(property);
        }

        public override void StopTesting()
        {
            m_InitialValue = null;
            EditorApplication.update -= UpdateCompletion;
        }

        

        protected override bool EvaluateCompletion()
        {

            InitializeRequiredStateIfNeeded();

            if (m_InitialValue == null)
                return false;

            if (m_MaterialPropertyPath == null || target.ReferencedObject == null)
                return false;

            var property = FindProperty(m_MaterialPropertyPath, (Material)target.ReferencedObject);

            if (property == null)
                return false;

            var currentValue = GetPropertyValueToString(property);

            if (currentValue != m_InitialValue)
                return true;

            return false;
        }

        public override bool AutoComplete()
        {
            throw new NotImplementedException();
        }
    }
}