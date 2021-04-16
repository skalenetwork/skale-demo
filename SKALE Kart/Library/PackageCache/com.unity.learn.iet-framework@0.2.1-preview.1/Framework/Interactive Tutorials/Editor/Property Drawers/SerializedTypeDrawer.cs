using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    [CustomPropertyDrawer(typeof(SerializedType))]
    class SerializedTypeDrawer : PropertyDrawer
    {
        private const string k_TypeNamePath = "m_TypeName";

        private static GUIStyle preDropGlow
        {
            get
            {
                if (s_PreDropGlow == null)
                {
                    s_PreDropGlow = new GUIStyle(GUI.skin.GetStyle("TL SelectionButton PreDropGlow"));
                    s_PreDropGlow.stretchHeight = true;
                    s_PreDropGlow.stretchWidth = true;
                }
                return s_PreDropGlow;
            }
        }
        private static GUIStyle s_PreDropGlow;

        class Options
        {
            public GUIContent[] displayedOptions;
            public string[] assemblyQualifiedNames;
            public bool dragging;

            public Options(Type baseType)
            {
                var allowedTypes = new HashSet<Type>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly == null)
                        continue;

                    try
                    {
                        foreach (var type in assembly.GetTypes())
                        {
                            if (baseType.IsAssignableFrom(type))
                                allowedTypes.Add(type);
                        }
                    }
                    catch (ReflectionTypeLoadException)
                    {
                    }
                }

                allowedTypes.Remove(baseType);

                var optionCount = allowedTypes.Count() + 1;
                displayedOptions = new GUIContent[optionCount];
                assemblyQualifiedNames = new string[optionCount];

                var index = 0;
                displayedOptions[index] = new GUIContent(string.Format("None ({0})", baseType.FullName));
                assemblyQualifiedNames[index] = "";
                index++;

                foreach (var allowedType in allowedTypes.OrderBy(t => t.FullName))
                {
                    displayedOptions[index] = new GUIContent(allowedType.FullName);
                    assemblyQualifiedNames[index] = allowedType.AssemblyQualifiedName;
                    index++;
                }
            }
        }

        Dictionary<string, Options> m_PropertyPathToOptions = new Dictionary<string, Options>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Options options;
            if (!m_PropertyPathToOptions.TryGetValue(property.propertyPath, out options))
            {
                var filterAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SerializedTypeFilterAttributeBase), true) as SerializedTypeFilterAttributeBase;
                options = new Options(filterAttribute.baseType);
                m_PropertyPathToOptions[property.propertyPath] = options;
            }

            var typeNameProperty = property.FindPropertyRelative(k_TypeNamePath);
            var selectedIndex = ArrayUtility.IndexOf(options.assemblyQualifiedNames, typeNameProperty.stringValue);
            if (selectedIndex == -1)
                selectedIndex = 0;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            var newIndex = EditorGUI.Popup(position, label, selectedIndex, options.displayedOptions);
            HandleDraggingToPopup(position, options, ref newIndex, property, typeNameProperty);
            if (EditorGUI.EndChangeCheck())
                typeNameProperty.stringValue = options.assemblyQualifiedNames[newIndex];

            EditorGUI.EndProperty();
        }

        private void RebuildOptions(SerializedProperty property)
        {
            m_PropertyPathToOptions.Remove(property.propertyPath);
        }

        private void HandleDraggingToPopup(Rect dropPosition, Options options, ref int index, SerializedProperty property, SerializedProperty typeNameProperty)
        {
            if (dropPosition.Contains(Event.current.mousePosition))
            {
                switch (Event.current.type)
                {
                    case EventType.DragExited:
                        options.dragging = false;
                        RebuildOptions(property);
                        if (GUI.enabled)
                            HandleUtility.Repaint();
                        break;

                    case EventType.DragPerform:
                    case EventType.DragUpdated:
                        options.dragging = true;
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        if (Event.current.type == EventType.DragPerform)
                        {
                            UnityObject selection = DragAndDrop.objectReferences.FirstOrDefault(o => o != null);
                            if (selection != null)
                            {
                                var type = selection.GetType();
                                if (type == null)
                                    index = 0;
                                else
                                {
                                    index = ArrayUtility.IndexOf(options.assemblyQualifiedNames, type.AssemblyQualifiedName);
                                    if (index == -1)
                                        index = 0;
                                }

                                GUI.changed = true;
                                DragAndDrop.AcceptDrag();
                                DragAndDrop.activeControlID = 0;
                                Event.current.Use();
                            }
                        }
                        break;
                }
            }
            else
            {
                if (options.dragging)
                {
                    if (GUI.enabled)
                        HandleUtility.Repaint();
                }
                options.dragging = false;
            }
            if (options.dragging)
                GUI.Box(dropPosition, "", preDropGlow);
        }
    }
}
