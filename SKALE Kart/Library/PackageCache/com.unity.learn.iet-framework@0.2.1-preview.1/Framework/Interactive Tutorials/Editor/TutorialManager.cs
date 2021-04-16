using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.InteractiveTutorials
{
    class TutorialManager : ScriptableObject
    {
        internal const string k_OriginalLayoutPath = "Temp/OriginalLayout.dwlt";
        const string k_DefaultsFolder = "Tutorial Defaults";

        static TutorialManager s_TutorialManager;
        public static TutorialManager instance
        {
            get
            {
                if (s_TutorialManager == null)
                {
                    s_TutorialManager = Resources.FindObjectsOfTypeAll<TutorialManager>().FirstOrDefault();
                    if (s_TutorialManager == null)
                    {
                        s_TutorialManager = CreateInstance<TutorialManager>();
                        s_TutorialManager.hideFlags = HideFlags.HideAndDontSave;
                    }
                }

                return s_TutorialManager;
            }
        }

        Tutorial m_Tutorial;

        public static bool IsLoadingLayout { get; private set; }

        public static event Action aboutToLoadLayout;
        public static event Action<bool> layoutLoaded; // bool == successful

        internal static TutorialWindow GetTutorialWindow()
        {
            return Resources.FindObjectsOfTypeAll<TutorialWindow>().FirstOrDefault();
        }

        public void StartTutorial(Tutorial tutorial)
        {
            if (tutorial == null)
            {
                Debug.LogError("Null Tutorial.");
                return;
            }

            // NOTE maximizeOnPlay=true was causing problems at some point
            // (tutorial was closed for some reason) but that problem seems to be gone.
            // Keeping this here in case the problem returns.
            //GameViewProxy.maximizeOnPlay = false;

            // Prevent Game view flashing briefly when starting tutorial.
            EditorWindow.GetWindow<SceneView>().Focus();

            // Is the previous tutorial finished? Make sure to record the progress.
            // by trying to progress to the next page which will take care of it.
            if (m_Tutorial && m_Tutorial.completed)
                m_Tutorial.TryGoToNextPage();

            m_Tutorial = tutorial;

            // Ensure we are in edit mode
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                EditorApplication.playModeStateChanged += PostponeStartTutorialToEditMode;
            }
            else
                StartTutorialInEditMode();
        }

        void PostponeStartTutorialToEditMode(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.playModeStateChanged -= PostponeStartTutorialToEditMode;
                StartTutorialInEditMode();
            }
        }

        void StartTutorialInEditMode()
        {
            // TODO HACK double delay to resolve various issue (e.g. black screen during save modifications dialog
            // Revisit and fix properly.
            EditorApplication.delayCall += delegate
            {
                EditorApplication.delayCall += delegate
                {
                    if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        return;

                    // TODO document why this is done
                    EditorWindow.GetWindow<SceneView>().Focus();

                    SaveOriginalScenes();
                    SaveOriginalWindowLayout();

                    m_Tutorial.LoadWindowLayout();

                    // Ensure TutorialWindow is open and set the current tutorial
                    var tutorialWindow = EditorWindow.GetWindow<TutorialWindow>();
                    tutorialWindow.SetTutorial(m_Tutorial);

                    m_Tutorial.ResetProgress();

                // Do not overwrite workspace in authoring mode, use version control instead.
                if (!ProjectMode.IsAuthoringMode())
                    LoadTutorialDefaultsIntoAssetsFolder();
                };
            };
        }

        public void RestoreOriginalState()
        {
            RestoreOriginalScenes();
            RestoreOriginalWindowLayout();
        }

        public void ResetTutorial()
        {
            var tutorialWindow = GetTutorialWindow();
            if (tutorialWindow == null || tutorialWindow.currentTutorial == null)
                return;

            m_Tutorial = tutorialWindow.currentTutorial;

            // Ensure we are in edit mode
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                EditorApplication.playModeStateChanged += PostponeResetTutorialToEditMode;
            }
            else
                StartTutorialInEditMode();
        }

        void PostponeResetTutorialToEditMode(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.playModeStateChanged -= PostponeStartTutorialToEditMode;
                ResetTutorialInEditMode();
            }
        }

        void ResetTutorialInEditMode()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            m_Tutorial.LoadWindowLayout();
            m_Tutorial.ResetProgress();

            // Do not overwrite workspace in authoring mode, use version control instead.
            if (!ProjectMode.IsAuthoringMode())
                LoadTutorialDefaultsIntoAssetsFolder();
        }

        static void SaveOriginalWindowLayout()
        {
            WindowLayoutProxy.SaveWindowLayout(k_OriginalLayoutPath);
        }

        internal void RestoreOriginalWindowLayout()
        {
            if (File.Exists(k_OriginalLayoutPath))
            {
                LoadWindowLayout(k_OriginalLayoutPath);
                File.Delete(k_OriginalLayoutPath);
            }
        }

        public static bool LoadWindowLayout(string path)
        {
            IsLoadingLayout = true;
            aboutToLoadLayout?.Invoke();
            var successful = EditorUtility.LoadWindowLayout(path);
            if (!successful)
                Debug.LogError($"Failed to load layout from \"{path}\".");
            layoutLoaded?.Invoke(successful);
            IsLoadingLayout = false;
            return successful;
        }

        [Serializable]
        struct SceneInfo
        {
            public string assetPath;
            public bool wasLoaded;
        }

        string m_OriginalActiveSceneAssetPath;
        SceneInfo[] m_OriginalScenes;

        // Saves current state of open/loaded scenes so we can restore later
        void SaveOriginalScenes()
        {
            m_OriginalActiveSceneAssetPath = SceneManager.GetActiveScene().path;
            m_OriginalScenes = new SceneInfo[SceneManager.sceneCount];
            for (var sceneIndex = 0; sceneIndex < m_OriginalScenes.Length; sceneIndex++)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);
                m_OriginalScenes[sceneIndex] = new SceneInfo
                {
                    assetPath = scene.path,
                    wasLoaded = scene.isLoaded,
                };
            }
        }

        internal void RestoreOriginalScenes( )
        {

            // Don't restore scene state if we didn't save it in the first place
            if (string.IsNullOrEmpty(m_OriginalActiveSceneAssetPath))
                return;

            // Exit play mode so we can open scenes (without necessarily loading them)
            EditorApplication.isPlaying = false;

            foreach (var sceneInfo in m_OriginalScenes)
            {
                // Don't open scene if path is empty (this is the case for a new unsaved unmodified scene)
                if (sceneInfo.assetPath == string.Empty)
                    continue;

                var openSceneMode = sceneInfo.wasLoaded ? OpenSceneMode.Additive : OpenSceneMode.AdditiveWithoutLoading;
                EditorSceneManager.OpenScene(sceneInfo.assetPath, openSceneMode);
            }

            var originalScenePaths = m_OriginalScenes.Select(sceneInfo => sceneInfo.assetPath).ToArray();
            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);

                // Set originally active scene
                if (scene.path == m_OriginalActiveSceneAssetPath)
                {
                    SceneManager.SetActiveScene(scene);
                    continue;
                }

                // Close scene if was not opened originally
                if (!originalScenePaths.Contains(scene.path))
                    EditorSceneManager.CloseScene(scene, true);
            }

            m_OriginalActiveSceneAssetPath = null;
        }

        static void LoadTutorialDefaultsIntoAssetsFolder()
        {
            if (!TutorialProjectSettings.instance.restoreDefaultAssetsOnTutorialReload)
                return;

            AssetDatabase.SaveAssets();
            string defaultsPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, k_DefaultsFolder);
            var dirtyMetaFiles = new HashSet<string>();
            DirectoryCopy(defaultsPath, Application.dataPath, dirtyMetaFiles);
            AssetDatabase.Refresh();
            int startIndex = Application.dataPath.Length - "Assets".Length;
            foreach (var dirtyMetaFile in dirtyMetaFiles)
                AssetDatabase.ImportAsset(Path.ChangeExtension(dirtyMetaFile.Substring(startIndex), null));
        }

        internal static void WriteAssetsToTutorialDefaultsFolder()
        {
            if (!TutorialProjectSettings.instance.restoreDefaultAssetsOnTutorialReload)
                return;

            if (EditorApplication.isPlaying)
            {
                Debug.LogError("Defaults cannot be written during play mode");
                return;
            }

            string defaultsPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, k_DefaultsFolder);
            DirectoryInfo defaultsDirectory = new DirectoryInfo(defaultsPath);
            if (defaultsDirectory.Exists)
            {
                foreach (var file in defaultsDirectory.GetFiles())
                    file.Delete();
                foreach (var directory in defaultsDirectory.GetDirectories())
                    directory.Delete(true);
            }
            DirectoryCopy(Application.dataPath, defaultsPath, null);
        }

        static void DirectoryCopy(string sourceDirectory, string destinationDirectory, HashSet<string> dirtyMetaFiles)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirectory);

            // Abort if source directory doesn't exist
            if (!dir.Exists)
                return;

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationDirectory, file.Name);
                if (dirtyMetaFiles != null && string.Equals(Path.GetExtension(tempPath), ".meta", StringComparison.OrdinalIgnoreCase))
                {
                    if (!File.Exists(tempPath) || !File.ReadAllBytes(tempPath).SequenceEqual(File.ReadAllBytes(file.FullName)))
                        dirtyMetaFiles.Add(tempPath);
                }
                file.CopyTo(tempPath, true);
            }

            // copy sub directories and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destinationDirectory, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, dirtyMetaFiles);
            }
        }
    }
}
