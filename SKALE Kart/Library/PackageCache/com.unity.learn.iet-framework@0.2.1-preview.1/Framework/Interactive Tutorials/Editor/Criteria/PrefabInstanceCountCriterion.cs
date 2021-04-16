using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class PrefabInstanceCountCriterion : Criterion
    {
        public enum InstanceCountComparison
        {
            AtLeast,
            Exactly,
            NoMoreThan,
        }

        public GameObject prefabParent;
        public InstanceCountComparison comparisonMode = InstanceCountComparison.AtLeast;
        [Range(0, 100)]
        public int instanceCount = 1;
        [SerializeField, HideInInspector]
        FutureObjectReference m_FutureReference;

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
            if (prefabParent == null)
                return false;

            var matches = FindObjectsOfType<GameObject>().Where(go => PrefabUtilityShim.GetCorrespondingObjectFromSource(go) == prefabParent);
            var count = matches.Count();
            switch (comparisonMode)
            {
                case InstanceCountComparison.AtLeast:
                    return count >= instanceCount;

                case InstanceCountComparison.Exactly:
                    var complete = count == instanceCount;
                    if (complete && instanceCount == 1 && m_FutureReference != null)
                        m_FutureReference.sceneObjectReference.Update(matches.First());
                    return complete;

                case InstanceCountComparison.NoMoreThan:
                    return count <= instanceCount;

                default:
                    return false;
            }
        }

        protected override IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            if (m_FutureReference == null)
                yield break;

            yield return m_FutureReference;
        }

        protected override void OnValidate()
        {
            // Destroy unreferenced future reference assets
            base.OnValidate();

            // Update future reference
            var needsUpdate = false;
            if (comparisonMode == InstanceCountComparison.Exactly && instanceCount == 1)
            {
                if (m_FutureReference == null)
                {
                    m_FutureReference = CreateFutureObjectReference();
                    m_FutureReference.referenceName = "Prefab Instance";
                    needsUpdate = true;
                }
            }
            else
                DestroyImmediate(m_FutureReference, true);

            if (needsUpdate)
                UpdateFutureObjectReferenceNames();
        }

        public override bool AutoComplete()
        {
            var prefabInstances = FindObjectsOfType<GameObject>().Where(go => PrefabUtilityShim.GetCorrespondingObjectFromSource(go) == prefabParent);
            var actualInstanceCount = prefabInstances.Count();
            var difference = actualInstanceCount - instanceCount;

            if (difference == 0)
                return true;

            switch (comparisonMode)
            {
                case InstanceCountComparison.AtLeast:
                    difference = Math.Min(0, difference);
                    break;

                case InstanceCountComparison.NoMoreThan:
                    difference = Math.Max(0, difference);
                    break;
            }

            if (difference < 0)
            {
                for (var i = 0; i < -difference; i++)
                    PrefabUtility.InstantiatePrefab(prefabParent);
            }
            else
            {
                foreach (var prefabInstance in prefabInstances.Take(difference))
                    DestroyImmediate(prefabInstance);
            }

            return true;
        }
    }
}
