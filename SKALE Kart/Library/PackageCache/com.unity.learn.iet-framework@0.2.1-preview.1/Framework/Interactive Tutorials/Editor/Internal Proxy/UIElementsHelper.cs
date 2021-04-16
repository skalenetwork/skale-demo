using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using VisualContainer = UnityEngine.UIElements.VisualElement;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace Unity.InteractiveTutorials
{
    // Handle difference in UIElements API between 2017.2 and above
    // Initialize on load to surface potential reflection issues immediately
    [InitializeOnLoad]
    public static class UIElementsHelper
    {
        static MethodInfo s_AddMethod;
        static MethodInfo s_RemoveMethod;
        static PropertyInfo s_ParentProperty;
        static PropertyInfo s_VisualTreeProperty;

        static UIElementsHelper()
        {
            s_AddMethod = GetMethod<VisualElement>("Add", typeof(VisualElement)) ?? GetMethod<VisualContainer>("AddChild", typeof(VisualElement));
            if (s_AddMethod == null)
                Debug.LogError("Cannot find method VisualContainer.AddChild/VisualElement.Add");

            s_RemoveMethod = GetMethod<VisualElement>("Remove", typeof(VisualElement)) ?? GetMethod<VisualContainer>("RemoveChild", typeof(VisualElement));
            if (s_RemoveMethod == null)
                Debug.LogError("Cannot find method VisualContainer.RemoveChild/VisualElement.Remove");

            s_ParentProperty = GetProperty<VisualElement>("parent", typeof(VisualElement)) ?? GetProperty<VisualElement>("parent", typeof(VisualContainer));
            if (s_ParentProperty == null)
                Debug.LogError("Cannot find property VisualElement.parent");

            s_VisualTreeProperty = GetProperty<GUIView>("visualTree", typeof(VisualElement)) ?? GetProperty<GUIView>("visualTree", typeof(VisualContainer));
            if (s_VisualTreeProperty == null)
                Debug.LogError("Cannot find property GUIView.visualTree");
        }

        static MethodInfo GetMethod<T>(string name, params Type[] parameterTypes)
        {
            return typeof(T).GetMethod(name,
                BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                parameterTypes,
                null
                );
        }

        static PropertyInfo GetProperty<T>(string name, Type returnType)
        {
            return typeof(T).GetProperty(name,
                BindingFlags.Public | BindingFlags.Instance,
                null,
                returnType,
                new Type[0],
                null
                );
        }

        public static void Add(VisualElement element, VisualElement child)
        {
            s_AddMethod.Invoke(element, new object[] { child });
        }

        public static void Add(GUIViewProxy view, VisualElement child)
        {
            if (view.IsDockedToEditor())
            {
                Add(GetVisualTree(view), child);
            }
            else
            {
                foreach (var visualElement in GetVisualTree(view).Children())
                {
                    Add(visualElement, child);
                }
            }
        }

        public static void Remove(VisualElement element, VisualElement child)
        {
            s_RemoveMethod.Invoke(element, new object[] { child });
        }

        public static VisualElement GetParent(VisualElement element)
        {
            return (VisualElement)s_ParentProperty.GetValue(element, new object[0]);
        }

        public static VisualElement GetVisualTree(GUIViewProxy guiViewProxy)
        {
            return (VisualElement)s_VisualTreeProperty.GetValue(guiViewProxy.guiView, new object[0]);
        }

        public static void SetLayout(this VisualElement element, Rect rect)
        {
#if UNITY_2019_1_OR_NEWER
            var style = element.style;
            style.position = Position.Absolute;
            style.marginLeft = 0.0f;
            style.marginRight = 0.0f;
            style.marginBottom = 0.0f;
            style.marginTop = 0.0f;
            style.left = rect.x;
            style.top = rect.y;
            style.right = float.NaN;
            style.bottom = float.NaN;
            style.width = rect.width;
            style.height = rect.height;
#else
            element.layout = rect;
#endif
        }
    }
}
