using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [InitializeOnLoad]
    public static class UserStartupCode
    {
        internal static void RunStartupCode()
        {
            var projectSettings = TutorialProjectSettings.instance;

            if (projectSettings.initialScene != null)
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(projectSettings.initialScene));

            TutorialManager.WriteAssetsToTutorialDefaultsFolder();

            if (projectSettings.startupTutorial != null)
                TutorialManager.instance.StartTutorial(projectSettings.startupTutorial);

            // Ensure Editor is in predictable state
            EditorPrefs.SetString("ComponentSearchString", string.Empty);
            Tools.current = Tool.Move;

            // Replace LastProjectPaths in window layouts used in tutorials so that e.g.
            // pre-saved Project window states work correctly.
            var readme = TutorialWindow.FindReadme();
            if (readme)
            {
                WindowLayoutProxy.ReplaceLastProjectPathWithCurrentProjectPath(readme.projectLayoutPath);
            }

            AssetDatabase.FindAssets($"t:{typeof(Tutorial).FullName}")
                .Select(guid =>
                    AssetDatabase.LoadAssetAtPath<Tutorial>(AssetDatabase.GUIDToAssetPath(guid)).windowLayoutPath
                )
                .Distinct()
                .ToList()
                .ForEach(layoutPath =>
                {
                    WindowLayoutProxy.ReplaceLastProjectPathWithCurrentProjectPath(layoutPath);
                });

            if (readme != null)
                readme.LoadTutorialProjectLayout();

            var welcomePage = FindWelcomePage();
            if (welcomePage != null)
            {
                TutorialModalWindow.TryToShow("Welcome", welcomePage, () => { });
            }

            var wnd = TutorialManager.GetTutorialWindow();
            if (wnd)
                wnd.showStartHereMarker = true;
        }

        internal static readonly string initFileMarkerPath = "InitCodeMarker";
        // Folder so that user can easily create this from the Editor's Project view.
        internal static readonly string dontRunInitCodeMarker = "Assets/DontRunInitCodeMarker";

        static UserStartupCode()
        {
            if (IsDontRunInitCodeMarkerSet())
                return;
            if (IsInitialized())
                return;

            EditorApplication.update += InitRunStartupCode;
        }

        private static void InitRunStartupCode()
        {
            SetInitialized();
            EditorApplication.update -= InitRunStartupCode;
            RunStartupCode();
        }

        public static bool IsInitialized()
        {
            return File.Exists(initFileMarkerPath);
        }

        private static bool IsDontRunInitCodeMarkerSet()
        {
            return Directory.Exists(dontRunInitCodeMarker);
        }

        public static void SetInitialized()
        {
            File.CreateText(initFileMarkerPath).Close();
        }

        internal static TutorialWelcomePage FindWelcomePage()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(TutorialWelcomePage).FullName}");
            if (guids.Length == 0)
            {
                return null;
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning("More than one TutorialWelcomePage found in the project.");
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<TutorialWelcomePage>(assetPath);
        }
    }
}
