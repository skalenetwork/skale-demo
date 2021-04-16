using System;
using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public static class HostViewProxy
    {
        public static event Action actualViewChanged;

        static HostViewProxy()
        {
            HostView.actualViewChanged += OnActualViewChanged;
        }

        static void OnActualViewChanged(HostView hostView)
        {
            if (actualViewChanged != null)
                actualViewChanged();
        }
    }
}
