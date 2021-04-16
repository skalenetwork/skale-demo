using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public abstract class Criterion : ScriptableObject
    {
        public static event Action<Criterion> criterionCompleted;
        public static event Action<Criterion> criterionInvalidated;

        readonly bool m_EvaluateCompletionOnAccess;
        bool m_Completed;

        public bool completed
        {
            get { return m_Completed; }
            protected set
            {
                if (value == m_Completed)
                    return;

                m_Completed = value;
                if (m_Completed)
                {
                    if (criterionCompleted != null)
                        criterionCompleted(this);
                }
                else
                {
                    if (criterionInvalidated != null)
                        criterionInvalidated(this);
                }
            }
        }

        public void ResetCompletionState()
        {
            m_Completed = false;
        }

        public virtual void StartTesting()
        {
        }

        public virtual void StopTesting()
        {
        }

        public virtual void UpdateCompletion()
        {
            completed = EvaluateCompletion();
        }

        protected virtual bool EvaluateCompletion()
        {
            throw new NotImplementedException((String.Format("Missing implementation of EvaluateCompletion in: {0}", GetType())));
        }

        protected virtual IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            return Enumerable.Empty<FutureObjectReference>();
        }

        protected virtual void OnValidate()
        {
            // Find instanceIDs of referenced future references
            var referencedFutureReferenceInstanceIDs = new HashSet<int>();
            foreach (var futureReference in GetFutureObjectReferences())
                referencedFutureReferenceInstanceIDs.Add(futureReference.GetInstanceID());

            // Destroy unreferenced future references
            var assetPath = AssetDatabase.GetAssetPath(this);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (var asset in assets)
            {
                if (asset is FutureObjectReference
                    && ((FutureObjectReference)asset).criterion == this
                    && !referencedFutureReferenceInstanceIDs.Contains(asset.GetInstanceID()))
                    DestroyImmediate(asset, true);
            }
        }

        protected FutureObjectReference CreateFutureObjectReference()
        {
            return CreateFutureObjectReference("Future Reference");
        }

        protected FutureObjectReference CreateFutureObjectReference(string referenceName)
        {
            var futureReference = CreateInstance<FutureObjectReference>();
            futureReference.criterion = this;
            futureReference.referenceName = referenceName;

            var assetPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.AddObjectToAsset(futureReference, assetPath);

            return futureReference;
        }

        protected void UpdateFutureObjectReferenceNames()
        {
            // Update future reference names in next editor update due to AssetDatase interactions
            EditorApplication.update += UpdateFutureObjectReferenceNamesPostponed;
        }

        void UpdateFutureObjectReferenceNamesPostponed()
        {
            // Unsubscribe immediately since it should only be called once
            EditorApplication.update -= UpdateFutureObjectReferenceNamesPostponed;

            var assetPath = AssetDatabase.GetAssetPath(this);
            var tutorialPage = (TutorialPage)AssetDatabase.LoadMainAssetAtPath(assetPath);
            var futureReferences = AssetDatabase.LoadAllAssetsAtPath(assetPath)
                .Where(o => o is FutureObjectReference)
                .Cast<FutureObjectReference>();
            foreach (var futureReference in futureReferences)
                tutorialPage.UpdateFutureObjectReferenceName(futureReference);
        }

        public abstract bool AutoComplete();
    }
}
