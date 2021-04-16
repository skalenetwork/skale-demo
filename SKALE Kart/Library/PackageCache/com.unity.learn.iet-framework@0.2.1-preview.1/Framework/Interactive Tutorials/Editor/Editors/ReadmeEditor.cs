using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [CustomEditor(typeof(TutorialContainer))]
    public class TutorialContainerEditor : Editor
    {
        static readonly bool s_IsAuthoringMode = s_IsAuthoringMode = ProjectMode.IsAuthoringMode();

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button(TutorialWindowMenuItem.Item))
                TutorialWindow.CreateWindow();

            if (s_IsAuthoringMode)
                base.OnInspectorGUI();
        }
    }
}
