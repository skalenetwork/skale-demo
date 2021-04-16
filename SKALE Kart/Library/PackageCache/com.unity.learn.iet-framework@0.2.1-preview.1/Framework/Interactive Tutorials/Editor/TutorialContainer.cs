using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.InteractiveTutorials
{
    public class TutorialContainer : ScriptableObject
    {
        // The default layout used when a NUO project is started for the first time.
        // TODO IET unit test the the file exist.
        // TODO IET unit test the the layout contains TutorialWindow.
        private const string k_DefaultLayout =
            "Packages/com.unity.learn.iet-framework/Framework/Interactive Tutorials/TutorialInfo/Layout.wlt";
        // The original layout is copied into this.
        internal const string k_UserLayoutPath = "Temp/UserLayout.wlt";

        // Instead of icon we will use a header background for the new design.
        [FormerlySerializedAs("icon")]
        public Texture2D headerBackground;
        public string title = "";
        public string projectName = "";
        public string description = "";
        [Tooltip("Can be used the override the default layout of the IET framework.")]
        public UnityEngine.Object projectLayout = null;
        public Section[] sections;

        public string projectLayoutPath =>
            projectLayout != null ? AssetDatabase.GetAssetPath(projectLayout) : k_DefaultLayout;

        [Serializable]
        public class Section
        {
            public int orderInView;
            public string heading;
            public string text;
            public string linkText; // text is not shown for new-style section cards, but required to make the card the open the URL
            public string url;
            public string buttonText; // not used for new-style section cards
            [SerializeField]
            private Tutorial tutorial = null;
            public Texture2D image;
            public Texture2D completedImage;
            [NonSerialized]
            public bool tutorialCompleted;

            // TODO rename, cards are now buttons inherently
            public bool CanDrawButton => !string.IsNullOrEmpty(buttonText) && tutorial;

            public string TutorialId => tutorial?.lessonId ?? "";

            public string EditorPrefsKey => $"Unity.InteractiveTutorials.lesson{TutorialId}";

            public void StartTutorial()
            {
                TutorialManager.instance.StartTutorial(tutorial);
            }

            // returns true if the state was found from EditorPrefs
            public bool LoadState()
            {
                const string nonexisting = "NONEXISTING";
                var state = EditorPrefs.GetString(EditorPrefsKey, nonexisting);
                if (state == "")
                {
                    tutorialCompleted = false;
                }
                else if (state == "Finished")
                {
                    tutorialCompleted = true;
                }
                return state != nonexisting;
            }

            public void SaveState()
            {
                EditorPrefs.SetString(EditorPrefsKey, tutorialCompleted ? "Finished" : "");
            }

        }

        void OnValidate()
        {
            SortSections();
            for (int i = 0; i < sections.Length; ++i)
            {
                sections[i].orderInView = i * 2;
            }
        }

        void SortSections()
        {
            Array.Sort(sections, (x, y) => x.orderInView.CompareTo(y.orderInView));
        }

        public void LoadTutorialProjectLayout()
        {
            File.Copy(projectLayoutPath, k_UserLayoutPath, overwrite:true);
            TutorialManager.LoadWindowLayout(k_UserLayoutPath);
        }
    }
}
