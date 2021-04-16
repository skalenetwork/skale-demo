using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    enum PlayModeState
    {
        Any,
        Playing,
        NotPlaying
    }

    class TutorialPage : ScriptableObject
    {
        public static event Action<TutorialPage> criteriaCompletionStateTested;
        public static event Action<TutorialPage> tutorialPageMaskingSettingsChanged;
        public static event Action<TutorialPage> tutorialPageNonMaskingSettingsChanged;

        internal event Action<TutorialPage> playedCompletionSound;

        public bool hasMovedToNextPage { get { return m_HasMovedToNextPage; } }
        private bool m_HasMovedToNextPage;

        public bool allCriteriaAreSatisfied { get; private set; }

        [Obsolete]
        public string sectionTitle { get { return m_SectionTitle; } }
        [Header("Content")]
        [SerializeField]
        string m_SectionTitle = "";
        public IEnumerable<TutorialParagraph> paragraphs { get { return m_Paragraphs; } }
        [SerializeField]
        internal TutorialParagraphCollection m_Paragraphs = new TutorialParagraphCollection();

        public MaskingSettings currentMaskingSettings
        {
            get
            {
                MaskingSettings result = null;
                for (int i = 0, count = m_Paragraphs.count; i < count; ++i)
                {
                    result = m_Paragraphs[i].maskingSettings;
                    if (!m_Paragraphs[i].completed)
                        break;
                }
                return result;
            }
        }

        [Header("Initial Camera Settings")]
        [SerializeField]
        SceneViewCameraSettings m_CameraSettings = new SceneViewCameraSettings();

        [Header("Button Labels")]
        [SerializeField]
        string m_NextButton = "Next";
        public string nextButton
        {
            get { return m_NextButton; }
            set
            {
                if (m_NextButton != value)
                {
                    m_NextButton = value;
                    RaiseTutorialPageNonMaskingSettingsChangedEvent();
                }
            }
        }
        [SerializeField]
        string m_DoneButton = "Done";
        public string doneButton
        {
            get { return m_DoneButton; }
            set
            {
                if (m_DoneButton != value)
                {
                    m_DoneButton = value;
                    RaiseTutorialPageNonMaskingSettingsChangedEvent();
                }
            }
        }

        public string guid
        {
            get
            {
                return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
            }
        }

        [Header("Sounds")]
        [SerializeField]
        AudioClip m_CompletedSound = null;

        public bool autoAdvanceOnComplete { get { return m_autoAdvance; } set { m_autoAdvance = value; }}
        [Header("Auto advance on complete?")]
        [SerializeField]
        bool m_autoAdvance;

        public void RaiseTutorialPageMaskingSettingsChangedEvent()
        {
            if (tutorialPageMaskingSettingsChanged != null)
                tutorialPageMaskingSettingsChanged(this);
        }

        public void RaiseTutorialPageNonMaskingSettingsChangedEvent()
        {
            if (tutorialPageNonMaskingSettingsChanged != null)
                tutorialPageNonMaskingSettingsChanged(this);
        }

        static Queue<WeakReference<TutorialPage>> s_DeferedValidationQueue = new Queue<WeakReference<TutorialPage>>();

        static TutorialPage()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        static void OnEditorUpdate()
        {
            while (s_DeferedValidationQueue.Count != 0)
            {
                var weakPageReference = s_DeferedValidationQueue.Dequeue();
                TutorialPage page;
                if (weakPageReference.TryGetTarget(out page))
                {
                    if (page != null) //Taking into account "unity null"
                    {
                        page.SyncCriteriaAndFutureReferences();
                    }
                }
            }
        }

        void OnValidate()
        {
            // Defer synchronization of sub-assets to next editor update due to AssetDatabase interactions

            // Retaining a reference to this instance in OnValidate/OnEnable can cause issues on project load
            // The same object might be imported more than once and if it's referenced it won't be unloaded correctly
            // Use WeakReference instead of subscribing directly to EditorApplication.update to avoid strong reference

            s_DeferedValidationQueue.Enqueue(new WeakReference<TutorialPage>(this));
        }

        void SyncCriteriaAndFutureReferences()
        {
            // Find instanceIDs of referenced criteria
            var referencedCriteriaInstanceIDs = new HashSet<int>();
            foreach (var paragraph in paragraphs)
            {
                foreach (var typedCriterion in paragraph.criteria)
                {
                    if (typedCriterion.criterion != null)
                        referencedCriteriaInstanceIDs.Add(typedCriterion.criterion.GetInstanceID());
                }
            }

            // Destroy unreferenced criteria
            var assetPath = AssetDatabase.GetAssetPath(this);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            var criteria = assets.Where(o => o is Criterion).Cast<Criterion>();
            foreach (var criterion in criteria)
            {
                if (!referencedCriteriaInstanceIDs.Contains(criterion.GetInstanceID()))
                    DestroyImmediate(criterion, true);
            }

            // Update future reference names
            var futureReferences = assets.Where(o => o is FutureObjectReference).Cast<FutureObjectReference>();
            foreach (var futureReference in futureReferences)
            {
                if (futureReference.criterion == null
                    || !referencedCriteriaInstanceIDs.Contains(futureReference.criterion.GetInstanceID()))
                {
                    // Destroy future reference from unrefereced criteria
                    DestroyImmediate(futureReference, true);
                }
                else
                    UpdateFutureObjectReferenceName(futureReference);
            }
        }

        public void UpdateFutureObjectReferenceName(FutureObjectReference futureReference)
        {
            int paragraphIndex;
            int criterionIndex;
            if (GetIndicesForCriterion(futureReference.criterion, out paragraphIndex, out criterionIndex))
            {
                futureReference.name = string.Format("Paragraph {0}, Criterion {1}, {2}",
                        paragraphIndex + 1, criterionIndex + 1, futureReference.referenceName);
            }
        }

        bool GetIndicesForCriterion(Criterion criterion, out int paragraphIndex, out int criterionIndex)
        {
            paragraphIndex = 0;
            criterionIndex = 0;

            foreach (var paragraph in paragraphs)
            {
                foreach (var typedCriterion in paragraph.criteria)
                {
                    if (typedCriterion.criterion == criterion)
                        return true;

                    criterionIndex++;
                }

                paragraphIndex++;
            }

            return false;
        }

        internal void Initiate()
        {
            SetupCompletionRequirements();
            if (m_CameraSettings != null && m_CameraSettings.enabled)
                m_CameraSettings.Apply();
        }

        public void ResetUserProgress()
        {
            RemoveCompletionRequirements();
            foreach (var paragraph in paragraphs)
            {
                if (paragraph.type == ParagraphType.Instruction)
                {
                    foreach (var criteria in paragraph.criteria)
                    {
                        if (criteria != null && criteria.criterion != null)
                        {
                            criteria.criterion.ResetCompletionState();
                            criteria.criterion.StopTesting();
                        }
                    }
                }
            }
            allCriteriaAreSatisfied = false;
            m_HasMovedToNextPage = false;
        }

        internal void SetupCompletionRequirements()
        {
            ValidateCriteria();
            if (hasMovedToNextPage)
                return;

            Criterion.criterionCompleted += OnCriterionCompleted;
            Criterion.criterionInvalidated += OnCriterionInvalidated;

            foreach (var paragraph in paragraphs)
            {
                if (paragraph.criteria != null)
                {
                    foreach (var criterion in paragraph.criteria)
                    {
                        if (criterion.criterion)
                            criterion.criterion.StartTesting();
                    }
                }
            }
        }

        internal void RemoveCompletionRequirements()
        {
            Criterion.criterionCompleted -= OnCriterionCompleted;
            Criterion.criterionInvalidated -= OnCriterionInvalidated;

            foreach (var paragraph in paragraphs)
            {
                if (paragraph.criteria != null)
                {
                    foreach (var criterion in paragraph.criteria)
                    {
                        if (criterion.criterion)
                        {
                            criterion.criterion.StopTesting();
                        }
                    }
                }
            }
        }

        void OnCriterionCompleted(Criterion sender)
        {
            if (!m_Paragraphs.Any(p => p.criteria.Any(c => c.criterion == sender)))
                return;

            if (sender.completed)
            {
                int paragraphIndex, criterionIndex;
                if (GetIndicesForCriterion(sender, out paragraphIndex, out criterionIndex))
                {
                    // only play sound effect and clear undo if all preceding criteria are already complete
                    var playSoundEffect = true;
                    for (int i = 0; i < paragraphIndex; ++i)
                    {
                        if (!m_Paragraphs[i].criteria.All(c => c.criterion.completed))
                        {
                            playSoundEffect = false;
                            break;
                        }
                    }
                    if (playSoundEffect)
                    {
                        Undo.ClearAll();
                        if (m_CompletedSound != null)
                            AudioUtilProxy.PlayClip(m_CompletedSound);
                        if (playedCompletionSound != null)
                            playedCompletionSound(this);
                    }
                }
            }
            ValidateCriteria();
        }

        void OnCriterionInvalidated(Criterion sender)
        {
            if (m_Paragraphs.Any(p => p.criteria.Any(c => c.criterion == sender)))
                ValidateCriteria();
        }

        internal void ValidateCriteria()
        {
            allCriteriaAreSatisfied = true;

            foreach (var paragraph in paragraphs)
            {
                if (paragraph.type == ParagraphType.Instruction)
                {
                    if (!paragraph.completed)
                    {
                        allCriteriaAreSatisfied = false;
                        break;
                    }
                }

                if (!allCriteriaAreSatisfied)
                    break;
            }

            if (criteriaCompletionStateTested != null)
                criteriaCompletionStateTested(this);
        }

        public void OnPageCompleted()
        {
            RemoveCompletionRequirements();
            m_HasMovedToNextPage = true;
        }
    }
}
