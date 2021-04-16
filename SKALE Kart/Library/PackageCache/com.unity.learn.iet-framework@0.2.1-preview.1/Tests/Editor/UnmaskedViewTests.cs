using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEditor.UIAutomation;

namespace Unity.InteractiveTutorials.Tests
{
    public class UnmaskedViewTests
    {
        [Test]
        [Ignore("Annoyingly closes Test Runner window and clears test results of other tests")]
        public void TestGetViewsAndRects_ThrowsArgumentException_WhenTryingToGetRectsFromTwoEditorWindowsInTheSameDockArea()
        {
            Assert.True(
                EditorUtility.LoadWindowLayout("Packages/com.unity.learn.iet-framework/Tests/Editor/UnmaskedViewTestLayout.dwlt"),
                "UnmaskedViewTestLayout.dwlt missing."
            );

            // these two windows are docked together in the test layout
            var unmaskedViews = new[] {
                UnmaskedView.CreateInstanceForEditorWindow<SceneView>(),
                UnmaskedView.CreateInstanceForEditorWindow<GameView>(),
            };

            Assert.Throws<ArgumentException>(
                () => UnmaskedView.GetViewsAndRects(unmaskedViews),
                "Did not throw ArgumentException when getting rects for two EditorWindows in the same DockArea"
            );
        }

        [Ignore("TODO Runs fine locally, fails on Yamato.")]
        [Test]
        public void TestGetViewsAndRects_ForNamedControlsInToolbar()
        {
            var unmaskedViews = new[] {
                UnmaskedView.CreateInstanceForGUIView<Toolbar>(
                    new[] {
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPersistentToolsPan" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPersistentToolsTranslate" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPersistentToolsRotate" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPersistentToolsScale" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPersistentToolsRect" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarToolPivotPositionButton" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarToolPivotOrientationButton" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPlayModePlayButton" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPlayModePauseButton" },
                    new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPlayModeStepButton" },
                }
                    )
            };

            var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews).m_MaskData;
            Assert.AreEqual(1, viewsAndRects.Count, "Did not find one view for the Toolbar");
            var rects = viewsAndRects.Values.First().rects;
            Assert.AreEqual(10, rects.Count, "Did not find all of the expected named controls in the Toolbar");
        }

        [UnityTest]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector()
        {
            var testObject = new GameObject("TestGetViewsAndRects_ForSerializedPropertiesInInspector");
            Selection.activeObject = testObject;
            try
            {
                EditorWindow.GetWindow<InspectorWindow>();
                yield return null;

                var unmaskedViews = new[] {
                    UnmaskedView.CreateInstanceForEditorWindow<InspectorWindow>(
                        new[] {
                        new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.Property, targetType = typeof(Transform), propertyPath = "m_LocalPosition" }
                    }
                        )
                };
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews).m_MaskData;
                Assert.AreEqual(1, viewsAndRects.Count, "Did not find one view for the Inspector");
                var rects = viewsAndRects.Values.First().rects;
                Assert.AreEqual(1, rects.Count, "Did not find exactly one control for the SerializedProperty m_LocalPosition for a Transform");
            }
            finally
            {
                GameObject.DestroyImmediate(testObject);
            }
        }

        [UnityTest]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector_WhenSamePathExistsOnMultipleComponents()
        {
            var testObject = new GameObject("TestGetViewsAndRects_ForSerializedPropertiesInInspector", typeof(Light), typeof(SpriteRenderer));

            Selection.activeObject = testObject;
            try
            {
                Assert.IsNotNull(new SerializedObject(testObject.GetComponent<Light>()).FindProperty("m_Color"));
                Assert.IsNotNull(new SerializedObject(testObject.GetComponent<SpriteRenderer>()).FindProperty("m_Color"));

                EditorWindow.GetWindow<InspectorWindow>();
                yield return null;

                var unmaskedViews = new[] {
                    UnmaskedView.CreateInstanceForEditorWindow<InspectorWindow>(
                        new[] {
                        new GUIControlSelector() { selectorMode = GUIControlSelector.Mode.Property, targetType = typeof(SpriteRenderer), propertyPath = "m_Color" }
                    }
                        )
                };
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews).m_MaskData;
                Assert.AreEqual(1, viewsAndRects.Count, "Did not find one view for the Inspector");
                var rects = viewsAndRects.Values.First().rects;
                Assert.AreEqual(1, rects.Count, "Did not find exactly one control for the SerializedProperty m_Color for a SpriteRenderer");
            }
            finally
            {
                GameObject.DestroyImmediate(testObject);
            }
        }

        [UnityTest]
        [TestCase(true, false, ExpectedResult = null)]
#if UNITY_2019_3_OR_NEWER
        [Ignore("TODO Fails on 2019.3 due two-pixel difference on the rect.")]
#endif
        [TestCase(false, true, ExpectedResult = null)]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector_WhenParentPropertyIsCollapsed(
            bool parentPropertyExpanded, bool expectedFoundAncestorProperty)
        {
            var testObject = new GameObject("TestGetViewsAndRects_ForContractedPropertyInInspector",
                typeof(TestComponents.ComponentWithNestedValues));

            Selection.activeObject = testObject;
            try
            {
                var serializedObject = new SerializedObject(testObject.GetComponent<TestComponents.ComponentWithNestedValues>());
                var parentProperty = serializedObject.FindProperty("componentWithNestedValuesFieldA");
                var childProperty = serializedObject.FindProperty(
                    "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB");

                Assert.That(parentProperty, Is.Not.Null);
                Assert.That(childProperty, Is.Not.Null);

                parentProperty.isExpanded = parentPropertyExpanded;
                serializedObject.ApplyModifiedProperties();

                var inspectorWindow = EditorWindow.GetWindow<InspectorWindow>();

                Rect labelRectOfExpectedFoundProperty;
                using (var automatedWindow = new AutomatedWindow<InspectorWindow>(inspectorWindow))
                {
                    inspectorWindow.Repaint();
                    yield return null;

                    if (expectedFoundAncestorProperty)
                    {
                        var parentElements = automatedWindow.FindElementsByGUIContent(
                            new GUIContent("Component With Nested Values Field A"));
                        Assert.That(parentElements.Count(), Is.EqualTo(1));
                        labelRectOfExpectedFoundProperty = parentElements.Single().rect;
                    }
                    else
                    {
                        var childElements = automatedWindow.FindElementsByGUIContent(
                            new GUIContent("Component With Nested Values Field B"));
                        Assert.That(childElements.Count(), Is.EqualTo(1));
                        labelRectOfExpectedFoundProperty = childElements.Single().rect;
                    }
                }

                var unmaskedViews = new[] {
                    UnmaskedView.CreateInstanceForEditorWindow<InspectorWindow>(
                        new[] {
                            new GUIControlSelector()
                            {
                                selectorMode = GUIControlSelector.Mode.Property,
                                targetType = typeof(TestComponents.ComponentWithNestedValues),
                                propertyPath = "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB"
                            }
                        }
                    )
                };

                bool foundAncestorProperty;
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews, out foundAncestorProperty)
                    .m_MaskData;

                Assert.That(foundAncestorProperty, Is.EqualTo(expectedFoundAncestorProperty));
                Assert.That(viewsAndRects.Count, Is.EqualTo(1), "Did not find one view for the Inspector");

                var rects = viewsAndRects.Values.First().rects;
                Assert.That(rects.Count, Is.EqualTo(1),
                    "Did not find exactly one control for the SerializedPropert " +
                    "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB for ComponentWithNestedValues");

                var rect = rects.Single();
                Assert.That(rect.yMin, Is.GreaterThanOrEqualTo(labelRectOfExpectedFoundProperty.yMin),
                    "Found property rect does not contain of label rect of expected found property");
                Assert.That(rect.yMax, Is.LessThanOrEqualTo(labelRectOfExpectedFoundProperty.yMax),
                    "Found property rect does not contain of label rect of expected found property");

            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(testObject);
            }
        }
    }
}
