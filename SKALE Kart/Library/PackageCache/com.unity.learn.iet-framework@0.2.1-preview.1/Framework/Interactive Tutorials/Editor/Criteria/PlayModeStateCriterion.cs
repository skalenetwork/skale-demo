using System;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class PlayModeStateCriterion : Criterion
    {
        enum PlayModeState
        {
            Playing,
            NotPlaying
        }

        [SerializeField]
        PlayModeState m_RequiredPlayModeState = PlayModeState.Playing;

        public override void StartTesting()
        {
            UpdateCompletion();

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public override void StopTesting()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    UpdateCompletion();
                    break;
            }
        }

        protected override bool EvaluateCompletion()
        {
            switch (m_RequiredPlayModeState)
            {
                case PlayModeState.NotPlaying:
                    return !EditorApplication.isPlaying;

                case PlayModeState.Playing:
                    return EditorApplication.isPlaying;

                default:
                    return false;
            }
        }

        public override bool AutoComplete()
        {
            EditorApplication.isPlaying = m_RequiredPlayModeState == PlayModeState.Playing;

            return true;
        }
    }
}
