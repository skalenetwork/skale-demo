using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public class EditorWindowProxy : EditorWindow
    {
        internal override void OnResized()
        {
            base.OnResized();
            OnResized_Internal();
        }

        protected virtual void OnResized_Internal()
        {
        }

        protected bool IsParentNull()
        {
            return m_Parent == null;
        }
    }
}
