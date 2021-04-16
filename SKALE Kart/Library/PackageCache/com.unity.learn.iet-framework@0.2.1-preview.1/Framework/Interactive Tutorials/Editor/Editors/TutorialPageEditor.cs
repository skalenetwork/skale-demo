using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [CustomEditor(typeof(TutorialPage))]
    class TutorialPageEditor : Editor
    {
        private const string k_ParagraphsPropertyPath = "m_Paragraphs.m_Items";
        private const string k_ParagraphMaskingSettingsRelativePropertyPath = "m_MaskingSettings";

        private static readonly Regex s_MatchMaskingSettingsPropertyPath =
            new Regex(
                string.Format(
                    "(^{0}\\.Array\\.size)|(^({0}\\.Array\\.data\\[\\d+\\]\\.{1}\\.))",
                    k_ParagraphsPropertyPath, k_ParagraphMaskingSettingsRelativePropertyPath
                    )
                );

        TutorialPage tutorialPage { get { return (TutorialPage)target; } }

        [NonSerialized]
        private string m_WarningMessage;

        protected virtual void OnEnable()
        {
            var paragraphs = serializedObject.FindProperty(k_ParagraphsPropertyPath);
            if (paragraphs == null)
            {
                m_WarningMessage = string.Format(
                        "Unable to locate property path {0} on this object. Automatic masking updates will not work.",
                        k_ParagraphsPropertyPath
                        );
            }
            else if (paragraphs.arraySize > 0)
            {
                var maskingSettings =
                    paragraphs.GetArrayElementAtIndex(0).FindPropertyRelative(k_ParagraphMaskingSettingsRelativePropertyPath);
                if (maskingSettings == null)
                    m_WarningMessage = string.Format(
                            "Unable to locate property path {0}.Array.data[0].{1} on this object. Automatic masking updates will not work.",
                            k_ParagraphsPropertyPath,
                            k_ParagraphMaskingSettingsRelativePropertyPath
                            );
            }

            Undo.postprocessModifications += OnPostprocessModifications;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        protected virtual void OnDisable()
        {
            Undo.postprocessModifications -= OnPostprocessModifications;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        private void OnUndoRedoPerformed()
        {
            if (tutorialPage != null)
                tutorialPage.RaiseTutorialPageMaskingSettingsChangedEvent();
        }

        private UndoPropertyModification[] OnPostprocessModifications(UndoPropertyModification[] modifications)
        {
            if (tutorialPage == null)
                return modifications;

            var targetModified = false;
            var maskingChanged = false;

            foreach (var modification in modifications)
            {
                if (modification.currentValue.target != target)
                    continue;

                targetModified = true;
                var propertyPath = modification.currentValue.propertyPath;
                if (s_MatchMaskingSettingsPropertyPath.IsMatch(propertyPath))
                {
                    maskingChanged = true;
                    break;
                }
            }

            if (maskingChanged)
                tutorialPage.RaiseTutorialPageMaskingSettingsChangedEvent();
            else if (targetModified)
                tutorialPage.RaiseTutorialPageNonMaskingSettingsChangedEvent();

            return modifications;
        }

        public override void OnInspectorGUI()
        {
            if (!string.IsNullOrEmpty(m_WarningMessage))
                EditorGUILayout.HelpBox(m_WarningMessage, MessageType.Warning);

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                TutorialWindow.GetWindow().ForceInititalizeTutorialAndPage();
            }
        }
    }
}
