using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class RequiredSelectionCriterion : Criterion
    {
        [Serializable]
        class ObjectReferenceCollection : CollectionWrapper<ObjectReference>
        {
        }

        [SerializeField]
        ObjectReferenceCollection m_ObjectReferences = new ObjectReferenceCollection();

        public void SetObjectReferences(IEnumerable<ObjectReference> objectReferences)
        {
            m_ObjectReferences.SetItems(objectReferences);
            UpdateCompletion();
        }

        public override void StartTesting()
        {
            UpdateCompletion();
            Selection.selectionChanged += UpdateCompletion;
        }

        public override void StopTesting()
        {
            Selection.selectionChanged -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            if (m_ObjectReferences.Count() != Selection.objects.Length)
                return false;

            foreach (var objectReference in m_ObjectReferences)
            {
                var referencedObject = objectReference.sceneObjectReference.ReferencedObject;
                if (referencedObject == null)
                    return false;

                if (!Selection.objects.Contains(referencedObject))
                    return false;
            }

            return true;
        }

        public override bool AutoComplete()
        {
            var referencedObjects = m_ObjectReferences.Select(or => or.sceneObjectReference.ReferencedObject);
            if (referencedObjects.Any(obj => obj == null))
            {
                Debug.LogWarning("Cannot auto-complete RequiredSelectionCriterion with unresolved object references");
                return false;
            }

            Selection.objects = referencedObjects.ToArray();
            return true;
        }
    }
}
