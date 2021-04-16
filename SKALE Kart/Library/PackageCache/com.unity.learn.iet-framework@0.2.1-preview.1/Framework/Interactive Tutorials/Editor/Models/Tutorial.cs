using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    class Tutorial : ScriptableObject
    {
        [Serializable]
        public class TutorialPageCollection : CollectionWrapper<TutorialPage>
        {
            public TutorialPageCollection() : base()
            {
            }

            public TutorialPageCollection(IList<TutorialPage> items) : base(items)
            {
            }
        }

        public static event Action<Tutorial> tutorialPagesModified;

        public string tutorialTitle { get { return m_TutorialTitle; } }
        [Header("Content")]
        [SerializeField]
        string m_TutorialTitle = "";

        public string lessonId { get { return m_LessonId; } }
        [SerializeField]
        string m_LessonId = "";

        public string version { get { return m_Version; } }
        [SerializeField]
        string m_Version = "0";

        [Header("Scene Data")]
        [SerializeField]
        UnityEditor.SceneAsset m_Scene = null;
        [SerializeField]
        SceneViewCameraSettings m_DefaultSceneCameraSettings = null;

        public enum ExitBehavior
        {
            ShowHomeWindow,
            CloseWindow,
        }

        public ExitBehavior exitBehavior { get { return m_ExitBehavior; } }
        [Header("Exit Behavior")]
        [SerializeField]
        ExitBehavior m_ExitBehavior = ExitBehavior.ShowHomeWindow;

        public enum SkipTutorialBehavior
        {
            SameAsExitBehavior,
            SkipToLastPage,
        }

        public SkipTutorialBehavior skipTutorialBehavior { get { return m_SkipTutorialBehavior; } }
        [SerializeField]
        SkipTutorialBehavior m_SkipTutorialBehavior = SkipTutorialBehavior.SameAsExitBehavior;

        public UnityEngine.Object assetSelectedOnExit { get { return m_AssetSelectedOnExit; } }
        [SerializeField]
        UnityEngine.Object m_AssetSelectedOnExit = null;

        public TutorialWelcomePage welcomePage { get { return m_WelcomePage; } }
        [Header("Pages"), SerializeField]
        private TutorialWelcomePage m_WelcomePage = null;
        [SerializeField]
        public TutorialWelcomePage completedPage { get { return m_CompletedPage; } }
        [SerializeField]
        private TutorialWelcomePage m_CompletedPage = null;

        // TODO Handle these states in a better fashion instead of boolean farming
        // e.g. typeof(currentPage) == WelcomePage
        [SerializeField]
        internal bool IsWelcomingPageShowing = true;
        [SerializeField]
        internal bool IsCompletedPageShowing = false;

        AutoCompletion m_AutoCompletion;

        public bool skipped { get { return m_Skipped; } }
        bool m_Skipped;

        public event Action tutorialInitiated;
        public event Action<TutorialPage, int> pageInitiated;
        public event Action<TutorialPage> goingBack;
        public event Action tutorialCompleted;

        public Tutorial()
        {
            m_AutoCompletion = new AutoCompletion(this);
        }

        void OnEnable()
        {
            m_AutoCompletion.OnEnable();
        }

        void OnDisable()
        {
            m_AutoCompletion.OnDisable();
        }

        [SerializeField, FormerlySerializedAs("m_Steps")]
        internal TutorialPageCollection m_Pages = new TutorialPageCollection();

        public IEnumerable<TutorialPage> pages { get { return m_Pages; } }

        public int currentPageIndex { get { return m_CurrentPageIndex; } }
        int m_CurrentPageIndex = 0;

        public TutorialPage currentPage
        {
            get
            {
                return m_Pages.count == 0
                    ? null
                    : m_Pages[m_CurrentPageIndex = Mathf.Min(m_CurrentPageIndex, m_Pages.count - 1)];
            }
        }

        public int pageCount { get { return m_Pages.count; } }

        public bool completed { get { return pageCount == 0 || (currentPageIndex == pageCount - 1 && currentPage != null && currentPage.allCriteriaAreSatisfied); } }

        [SerializeField, Tooltip("Saved layouts can be found in the following directories:\n" +
             "Windows: %APPDATA%/Unity/<version>/Preferences/Layouts\n" +
             "macOS: ~/Library/Preferences/Unity/<version>/Layouts\n" +
             "Linux: ~/.config/Preferences/Unity/<version>/Layouts")]
        UnityObject m_WindowLayout = null;

        internal string windowLayoutPath => AssetDatabase.GetAssetPath(m_WindowLayout);

        public bool isAutoCompleting { get { return m_AutoCompletion.running; } }

        public void StartAutoCompletion()
        {
            m_AutoCompletion.Start();
        }

        public void StopAutoCompletion()
        {
            m_AutoCompletion.Stop();
        }

        public void StopTutorial()
        {
            if (currentPage != null)
                currentPage.RemoveCompletionRequirements();
        }

        public void GoToPreviousPage()
        {
            if (m_CurrentPageIndex == 0 && !IsWelcomingPageShowing)
            {
                IsWelcomingPageShowing = true;
                return;
            }
            OnGoingBack(currentPage);
            m_CurrentPageIndex = Mathf.Max(0, m_CurrentPageIndex - 1);
            OnPageInitiated(currentPage, m_CurrentPageIndex);
        }

        public bool TryGoToNextPage()
        {
            if (!currentPage.allCriteriaAreSatisfied && !currentPage.hasMovedToNextPage)
                return false;
            if (m_Pages.count == m_CurrentPageIndex + 1)
            {
                OnTutorialCompleted();
                IsCompletedPageShowing = true;
                return false;
            }
            int newIndex = Mathf.Min(m_Pages.count - 1, m_CurrentPageIndex + 1);
            if (newIndex != m_CurrentPageIndex)
            {
                if (currentPage != null)
                    currentPage.OnPageCompleted();
                m_CurrentPageIndex = newIndex;
                OnPageInitiated(currentPage, m_CurrentPageIndex);
                return true;
            }
            return false;
        }

        public void RaiseTutorialPagesModified()
        {
            tutorialPagesModified?.Invoke(this);
        }

        void LoadScene()
        {
            // load scene
            if (m_Scene != null)
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(m_Scene));
            else
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);

            // move scene view camera into place
            if (m_DefaultSceneCameraSettings != null && m_DefaultSceneCameraSettings.enabled)
                m_DefaultSceneCameraSettings.Apply();
            OnTutorialInitiated();
            if (pageCount > 0)
                OnPageInitiated(currentPage, m_CurrentPageIndex);
        }

        internal void LoadWindowLayout()
        {
            if (m_WindowLayout == null)
                return;

            var layoutPath = AssetDatabase.GetAssetPath(m_WindowLayout);
            TutorialManager.LoadWindowLayout(layoutPath);
        }

        internal void ResetProgress()
        {
            foreach (var page in m_Pages)
            {
                page?.ResetUserProgress();
            }
            m_CurrentPageIndex = 0;
            IsWelcomingPageShowing = true;
            IsCompletedPageShowing = false;
            m_Skipped = false;
            LoadScene();
        }

        protected virtual void OnTutorialInitiated()
        {
            tutorialInitiated?.Invoke();
        }

        protected virtual void OnTutorialCompleted()
        {
            tutorialCompleted?.Invoke();
        }

        protected virtual void OnPageInitiated(TutorialPage page, int index)
        {
            page?.Initiate();
            pageInitiated?.Invoke(page, index);
        }

        protected virtual void OnGoingBack(TutorialPage page)
        {
            page?.RemoveCompletionRequirements();
            goingBack?.Invoke(page);
        }

        public void SkipToLastPage()
        {
            m_Skipped = true;
            m_CurrentPageIndex = pageCount - 1;
            OnPageInitiated(currentPage, m_CurrentPageIndex);
        }
    }
}
