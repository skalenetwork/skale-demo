using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public static class InspectorWindowProxy
    {
        public static void DirtyAllEditors(EditorWindow window)
        {
            var inspector = window as InspectorWindow;
            if (inspector != null && inspector.tracker != null)
            {
                foreach (var editor in inspector.tracker.activeEditors)
                    editor.isInspectorDirty = true;
            }
        }

        public static void ForceShowInspector()
        {
            EditorWindow.GetWindow<InspectorWindow>().Show();
        }
    }
}
