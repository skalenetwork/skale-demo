using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class GUIViewProxy
    {
        public static event Action<UnityObject> positionChanged;

        public static Type guiViewType { get { return typeof(GUIView); }}
        public static Type tooltipViewType { get { return typeof(TooltipView); }}

        static GUIViewProxy()
        {
            GUIView.positionChanged += OnPositionChanged;
        }

        static void OnPositionChanged(GUIView guiView)
        {
            if (positionChanged != null)
                positionChanged(guiView);
        }

        public static bool IsAssignableFrom(Type type)
        {
            return typeof(GUIView).IsAssignableFrom(type);
        }

        GUIView m_GUIView;

        internal GUIView guiView { get { return m_GUIView; } }

        public bool isValid { get { return m_GUIView != null; } }
        public bool isWindowAndRootViewValid { get { return m_GUIView.window != null && m_GUIView.window.rootView != null; } }
        public Rect position { get { return m_GUIView.position; } }

        internal GUIViewProxy(GUIView guiView)
        {
            m_GUIView = guiView;
        }

        public void Repaint()
        {
            m_GUIView.Repaint();
        }

        public void RepaintImmediately()
        {
            m_GUIView.RepaintImmediately();
        }

        public bool IsActualViewAssignableTo(Type editorWindowType)
        {
            var hostView = m_GUIView as HostView;
            return hostView != null && hostView.actualView != null && editorWindowType.IsInstanceOfType(hostView.actualView);
        }

        public bool IsDockedToEditor()
        {
            var hostView = m_GUIView as HostView;

            if (hostView != null && hostView.window != null)
            {
                return hostView.window.showMode == ShowMode.MainWindow;
            }

            return true;
        }

        public bool IsGUIViewAssignableTo(Type targetViewType)
        {
            return targetViewType.IsInstanceOfType(m_GUIView);
        }
    }

    public class GUIViewProxyComparer : IEqualityComparer<GUIViewProxy>
    {
        public bool Equals(GUIViewProxy x, GUIViewProxy y)
        {
            return x.guiView == y.guiView;
        }

        public int GetHashCode(GUIViewProxy obj)
        {
            return obj.guiView.GetHashCode();
        }
    }

    public static class EditorWindowExtension
    {
        public static GUIViewProxy GetParent(this EditorWindow window)
        {
            return new GUIViewProxy(window.m_Parent);
        }
    }
}
