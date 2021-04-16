using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class SceneAddedToBuildCriterion : Criterion
    {
        [SerializeField]
        SceneAsset m_Scene;

        public SceneAsset scene
        {
            get
            {
                return m_Scene;
            }
            set
            {
                m_Scene = value;
            }
        }

        public override void StartTesting()
        {
            UpdateCompletion();

            EditorBuildSettings.sceneListChanged += UpdateCompletion;
        }

        public override void StopTesting()
        {
            EditorBuildSettings.sceneListChanged -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            if (m_Scene == null)
            {
                return false;
            }
            if (m_Scene)
            {
                var scenePath = AssetDatabase.GetAssetPath(m_Scene);
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    if (scene.enabled)
                    {
                        if (scene.path == scenePath)
                            return true;
                    }
                }
            }
            return false;

        }

        public override bool AutoComplete()
        {
            if (m_Scene == null)
            {
                return false;
            }

            // Look if scene is already added in the build settings
            bool addedScene = SceneIsAddedToBuildSettings(m_Scene, true);
            if (addedScene)
            {
                return true;
            }

            // If the scene does not exist, we add it
            AddSceneToBuildSettings(m_Scene);

            return true;
        }

        public static bool SceneIsAddedToBuildSettings(SceneAsset asset, bool forceEnable = false)
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                return false;
            }
            var scenePath = AssetDatabase.GetAssetPath(asset);
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path == scenePath)
                {
                    if (forceEnable)
                    {
                        scene.enabled = true;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void AddSceneToBuildSettings(SceneAsset scene, bool enabled = true)
        {
            var scenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                scenes[i] = EditorBuildSettings.scenes[i];
            }

            scenes[scenes.Length - 1] = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(scene), enabled);

            EditorBuildSettings.scenes = scenes;
        }
    }
}