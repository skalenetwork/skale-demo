using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class ComponentAddedCriterion : Criterion
    {
        [SerializeField, FormerlySerializedAs("targetGameObject")]
        ObjectReference m_TargetGameObject;

        [SerializeField, FormerlySerializedAs("requiredComponents")]
        SerializedTypeCollection m_RequiredComponents = new SerializedTypeCollection();

        public GameObject targetGameObject
        {
            get
            {
                if (m_TargetGameObject == null)
                    return null;
                return m_TargetGameObject.sceneObjectReference.ReferencedObjectAsGameObject;
            }
            set
            {
                if (m_TargetGameObject == null)
                    m_TargetGameObject = new ObjectReference();
                m_TargetGameObject.sceneObjectReference.Update(value);
            }
        }

        public IList<Type> requiredComponents
        {
            get
            {
                return m_RequiredComponents.Select(typeAndFutureReference => typeAndFutureReference.serializedType.type)
                    .ToList();
            }
            set
            {
                var items = value.Select(type => new TypeAndFutureReference(new SerializedType(type))).ToList();
                m_RequiredComponents.SetItems(items);
                OnValidate();
            }
        }

        public override void StartTesting()
        {
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        public override void StopTesting()
        {
            EditorApplication.update -= UpdateCompletion;
        }

        protected override void OnValidate()
        {
            // Destroy unused future reference assets
            base.OnValidate();

            // Update future references
            var requiredComponentsIndex = 0;
            foreach (var typeAndFutureReference in m_RequiredComponents)
            {
                requiredComponentsIndex++;

                var type = typeAndFutureReference.serializedType.type;
                if (type == null)
                    continue;

                if (typeAndFutureReference.futureReference == null)
                    typeAndFutureReference.futureReference = CreateFutureObjectReference();

                typeAndFutureReference.futureReference.referenceName = string.Format("{0}: {1}",
                        requiredComponentsIndex, type.FullName);
            }

            if (requiredComponentsIndex != 0)
                UpdateFutureObjectReferenceNames();
        }

        protected override bool EvaluateCompletion()
        {
            var gameObject = targetGameObject;
            if (gameObject == null)
                return false;

            if (requiredComponents.Count() == 0)
                return true;

            foreach (var type in requiredComponents)
            {
                if (type == null)
                {
                    Debug.LogWarning("Testing for a null component type will always fail.");
                    return false;
                }

                if (gameObject.GetComponent(type) == null)
                    return false;
            }

            // Update future references
            foreach (var requiredType in m_RequiredComponents)
            {
                var component = gameObject.GetComponent(requiredType.serializedType.type);
                requiredType.futureReference.sceneObjectReference.Update(component);
            }

            return true;
        }

        protected override IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            return m_RequiredComponents
                .Where(c => c.serializedType.type != null && c.futureReference != null)
                .Select(c => c.futureReference);
        }

        public override bool AutoComplete()
        {
            var gameObject = targetGameObject;
            if (gameObject == null)
                return false;

            foreach (var type in requiredComponents)
            {
                var component = gameObject.AddComponent(type);
                if (component == null)
                    return false;
            }

            return true;
        }

        [Serializable]
        public class SerializedTypeCollection : CollectionWrapper<TypeAndFutureReference> {}

        [Serializable]
        public class TypeAndFutureReference : ICloneable
        {
            [SerializedTypeFilter(typeof(Component))]
            public SerializedType serializedType;
            public FutureObjectReference futureReference;

            public TypeAndFutureReference(SerializedType serializedType)
            {
                this.serializedType = serializedType;
            }

            public object Clone()
            {
                return new TypeAndFutureReference(serializedType);
            }
        }
    }
}
