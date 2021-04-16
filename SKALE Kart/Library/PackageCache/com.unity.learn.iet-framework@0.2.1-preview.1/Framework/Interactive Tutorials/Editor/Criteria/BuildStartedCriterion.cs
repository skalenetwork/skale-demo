using System;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class BuildStartedCriterion : Criterion
    {
        bool m_BuildStarted;

        public void BuildPlayerCustomHandler(BuildPlayerOptions options)
        {
            m_BuildStarted = true;
            BuildPipeline.BuildPlayer(options);
        }

        public override void StartTesting()
        {
            Action<BuildPlayerOptions> customHandler = BuildPlayerCustomHandler;
            m_BuildStarted = false;
            UpdateCompletion();
            BuildPlayerWindow.RegisterBuildPlayerHandler(customHandler);
            EditorApplication.update += UpdateCompletion;
        }

        public override void StopTesting()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(null);
            EditorApplication.update -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            return m_BuildStarted;
        }

        public override bool AutoComplete()
        {
            return true;
        }
    }
}