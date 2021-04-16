using System;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class SceneViewCameraMovedCriterion : Criterion
    {
        [NonSerialized]
        bool m_InitialPositionInitialized = false;
        [NonSerialized]
        Vector3 m_InitialCameraPosition;
        [NonSerialized]
        Quaternion m_InitialCameraOrientation;

        public override void StartTesting()
        {

            UpdateInitialCameraPositionIfNeeded();
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        void UpdateInitialCameraPositionIfNeeded()
        {
            if (m_InitialPositionInitialized)
                return;

            if(SceneView.lastActiveSceneView == null)
                return;

            m_InitialPositionInitialized = true;
            m_InitialCameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
            m_InitialCameraOrientation = SceneView.lastActiveSceneView.camera.transform.localRotation;
        }

        public override void StopTesting()
        {
            EditorApplication.update -= UpdateCompletion;
            m_InitialPositionInitialized = false;
        }

        protected override bool EvaluateCompletion()
        {
            if (SceneView.lastActiveSceneView == null)
                return false;

            UpdateInitialCameraPositionIfNeeded();
            var currentPosition = SceneView.lastActiveSceneView.camera.transform.position;
            var currentOrientation = SceneView.lastActiveSceneView.camera.transform.localRotation;
            return m_InitialCameraPosition != currentPosition || m_InitialCameraOrientation != currentOrientation;

        }

        public override bool AutoComplete()
        {
            SceneView.lastActiveSceneView.camera.transform.position = m_InitialCameraPosition + Vector3.back;
            return true;
        }
    }
}