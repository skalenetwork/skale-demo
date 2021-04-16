using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UIAutomation;
using UnityEngine;
using UnityEngine.TestTools;
using UnityObject = UnityEngine.Object;
using NUnit.Framework;

namespace Unity.InteractiveTutorials.Tests
{
    public class TutorialWindowTests
    {
        class MockCriterion : Criterion
        {
            public void Complete(bool complete)
            {
                completed = complete;
            }

            protected override bool EvaluateCompletion()
            {
                return completed;
            }

            public override bool AutoComplete()
            {
                return true;
            }
        }

        static float defaultAnimBoolSpeed { get { var animBool = new AnimBool(); return animBool.speed; } }

        private Tutorial m_Tutorial;
        private TutorialWindow m_Window;

        TutorialPage firstPage { get { return m_Tutorial.m_Pages[0]; } }
        TutorialPage secondPage { get { return m_Tutorial.m_Pages[1]; } }
        MockCriterion firstPageCriterion { get { return m_Tutorial.m_Pages[0].m_Paragraphs[0].m_Criteria[0].criterion as MockCriterion; } }
        string firstPageInstructionSummary { get { return string.Format("{0}-SUMMARY", TestContext.CurrentContext.Test.FullName); } }
        string firstPageInstructionText { get { return string.Format("{0}-TEXT", TestContext.CurrentContext.Test.FullName); } }
        string doneButtonText { get { return string.Format("{0}-DONE", TestContext.CurrentContext.Test.FullName); } }
        string nextButtonText { get { return string.Format("{0}-NEXT", TestContext.CurrentContext.Test.FullName); } }

        [SetUp]
        public void SetUp()
        {
            m_Tutorial = ScriptableObject.CreateInstance<Tutorial>();

            m_Tutorial.m_Pages = new Tutorial.TutorialPageCollection(
                    new[] { ScriptableObject.CreateInstance<TutorialPage>(), ScriptableObject.CreateInstance<TutorialPage>() }
                    );
            for (int i = 0; i < m_Tutorial.m_Pages.count; ++i)
            {
                m_Tutorial.m_Pages[i].name = string.Format("{0}-PAGE-{1}", TestContext.CurrentContext.Test.FullName, i + 1);
                m_Tutorial.m_Pages[i].doneButton = doneButtonText;
                m_Tutorial.m_Pages[i].nextButton = nextButtonText;
            }

            var paragraph = new TutorialParagraph
            {
                m_Type = ParagraphType.Instruction,
                m_Criteria = new TypedCriterionCollection(
                        new[] {
                    new TypedCriterion(new SerializedType(typeof(MockCriterion)), ScriptableObject.CreateInstance<MockCriterion>())
                }
                        )
            };
            paragraph.summary = firstPageInstructionSummary;
            paragraph.text = firstPageInstructionText;
            m_Tutorial.m_Pages[0].m_Paragraphs = new TutorialParagraphCollection(new[] { paragraph });

            m_Window = EditorWindow.GetWindow<TutorialWindow>();
            m_Window.showTabClosedDialog = false;
            m_Window.SetTutorial(m_Tutorial, reload: false);
        }

        [TearDown]
        public void TearDown()
        {
            if (m_Tutorial == null)
                return;

            foreach (var page in m_Tutorial.pages)
            {
                if (page == null)
                    continue;

                foreach (var paragraph in page.paragraphs)
                {
                    if (paragraph == null)
                        continue;

                    foreach (var criterion in paragraph.criteria)
                    {
                        if (criterion != null && criterion.criterion != null)
                            UnityObject.DestroyImmediate(criterion.criterion);
                    }
                }

                UnityObject.DestroyImmediate(page);
            }

            UnityObject.DestroyImmediate(m_Tutorial);

            if (m_Window != null)
                m_Window.Close();
        }

        static IAutomatedUIElement FindElementWithText(AutomatedWindow<TutorialWindow> automatedWindow, string text, string elementName, Action<object, string, object[]> assert = null)
        {
            var result = automatedWindow.FindElementsByGUIContent(new GUIContent(text)).LastOrDefault();
            assert = assert ?? Assert.IsNotNull;
            assert(result, string.Format("Finding {0} with expected text: \"{1}\"", elementName, text), null);
            return result;
        }

        static IAutomatedUIElement FindElementWithStyle(AutomatedWindow<TutorialWindow> automatedWindow, GUIStyle style, string elementName)
        {
            var result = automatedWindow.FindElementsByGUIStyle(style).FirstOrDefault();
            Assert.IsNotNull(result, string.Format("Finding {0} with expected style: {1}", elementName, style));
            return result;
        }

        [UnityTest]
        public IEnumerator CanClickNextButton_WhenRevistingCompletedPage_WhenItsCriteriaHaveBeenLaterInvalidated()
        {
            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();

                // next button should be disabled
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(firstPage, m_Window.currentTutorial.currentPage);

                // complete criterion; next button should now be enabled
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(secondPage, m_Window.currentTutorial.currentPage);

                // go back
                automatedWindow.Click(FindElementWithStyle(automatedWindow, AllTutorialStyles.backButton, "back button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(firstPage, m_Window.currentTutorial.currentPage);

                // invalidate criterion; next button should still be enabled
                firstPageCriterion.Complete(false);
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(secondPage, m_Window.currentTutorial.currentPage);
            }
        }

        [UnityTest]
        public IEnumerator ClickingBackButton_WhenPreviousPageHasAutoAdvanceOnCompleteSet_MovesToPreviousPage()
        {
            // let first page auto-advance on completion
            firstPage.autoAdvanceOnComplete = true;

            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();

                // complete criterion and auto-advance to next page, then press back button to come back
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithStyle(automatedWindow, AllTutorialStyles.backButton, "back button"));
                yield return null;
                m_Window.RepaintImmediately();

                // we should now be back at the first page again
                Assert.AreEqual(firstPage, m_Window.currentTutorial.currentPage);
            }
        }

        [UnityTest]
        public IEnumerator FirstInstructionIsExpandedAndInteractable_WhenPageLoads()
        {
            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                // yield once so that AnimBool for instruction text can expand
                yield return null;
                m_Window.RepaintImmediately();

                // instruction should be expanded by default
                FindElementWithText(automatedWindow, firstPageInstructionText, "instruction text", Assert.IsNotNull);

                // ensure text can be collapsed
                automatedWindow.Click(FindElementWithText(automatedWindow, firstPageInstructionSummary, "instruction summary"));
                yield return null;
                m_Window.RepaintImmediately();
                // wait for anim bool to expand
                var expandSpeed = defaultAnimBoolSpeed;
                var prevTime = DateTime.Now;
                var expanded = 0f;
                while (expanded < 1f)
                {
                    expanded += (float)(DateTime.Now - prevTime).TotalSeconds * expandSpeed;
                    prevTime = DateTime.Now;
                    yield return null;
                }
                yield return null;
                FindElementWithText(automatedWindow, firstPageInstructionText, "instruction text", Assert.IsNull);
            }
        }

        [UnityTest]
        public IEnumerator InstructionsAreExpandedAndDisabled_WhenRevisitingCompletedPage()
        {
            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                // complete criterion, go to next page, and then come back
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithStyle(automatedWindow, AllTutorialStyles.backButton, "back button"));
                yield return null;
                m_Window.RepaintImmediately();

                // instruction should now be expanded and user cannot collapse it
                FindElementWithText(automatedWindow, firstPageInstructionText, "instruction text", Assert.IsNotNull);
                automatedWindow.Click(FindElementWithText(automatedWindow, firstPageInstructionSummary, "instruction summary"));
                yield return null;
                m_Window.RepaintImmediately();
                FindElementWithText(automatedWindow, firstPageInstructionText, "instruction text", Assert.IsNotNull);
            }
        }

        [UnityTest]
        public IEnumerator InstructionsAreStyledAsCompleteButNotActive_WhenRevisitingCompletedPage()
        {
            m_Window.RepaintImmediately();

            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();
                // instruction should be styled as active
                FindElementWithStyle(automatedWindow, AllTutorialStyles.activeElementBackground, "active instruction");
                FindElementWithStyle(automatedWindow, AllTutorialStyles.instructionLabelIconNotCompleted, "incomplete instruction icon");

                // complete criterion and ensure it is styled as complete
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                FindElementWithStyle(automatedWindow, AllTutorialStyles.completedElementBackground, "completed instruction background");
                FindElementWithStyle(automatedWindow, AllTutorialStyles.instructionLabelIconCompleted, "completed instruction icon");

                // go to next page and then come back
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithStyle(automatedWindow, AllTutorialStyles.backButton, "back button"));
                yield return null;
                m_Window.RepaintImmediately();

                // ensure instruction is still marked as completed
                FindElementWithStyle(automatedWindow, AllTutorialStyles.completedElementBackground, "completed instruction background");
                FindElementWithStyle(automatedWindow, AllTutorialStyles.instructionLabelIconCompleted, "completed instruction icon");
            }
        }

        [Test]
        public void ApplyMasking_WhenPageIsActivated()
        {
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();
            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // the tutorial window and the specified unmasked view should both be unmasked
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == m_Window.m_Parent || view == Toolbar.get || view is TooltipView)
                    Assert.IsFalse(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
                else
                    Assert.IsTrue(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
            }
        }

        [UnityTest]
        public IEnumerator ApplyMasking_ToAllViewsExceptTutorialWindowAndTooltips_WhenRevisitingCompletedPage()
        {
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            firstPageCriterion.Complete(true);

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // masking of final instruction still applied when it is complete
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == m_Window.m_Parent || view == Toolbar.get || view is TooltipView)
                    Assert.IsFalse(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
                else
                    Assert.IsTrue(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
            }

            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();

                // go to next page and then come back
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithStyle(automatedWindow, AllTutorialStyles.backButton, "back button"));
                yield return null;
                m_Window.RepaintImmediately();
            }

            // now only tutorial window should be unmasked
            foreach (var view in views)
            {
                if (view == m_Window.m_Parent || view is TooltipView)
                    Assert.IsFalse(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
                else
                    Assert.IsTrue(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
            }
        }

        [Test]
        public void ApplyHighlighting_ToUnmaskedViews_WhenPageOnlyContainsNarrativeParagraphs()
        {
            firstPage.m_Paragraphs[0].m_Type = ParagraphType.Narrative;
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // only the specified unmasked view should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get)
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                else
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
            }
        }

        [Test]
        public void ApplyHighlighting_ToTutorialWindow_WhenAllTasksAreComplete()
        {
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            firstPageCriterion.Complete(true);

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // only the tutorial window should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == m_Window.m_Parent)
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                else
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
            }
        }

        [Test]
        public void ApplyHighlighting_ToOnlySpecifiedControls_WhenMaskingSettingsSpecifyControlsAndEntireWindowsAndViews()
        {
            var playButtonContrlSelector = new GUIControlSelector
            {
                selectorMode = GUIControlSelector.Mode.NamedControl, controlName = "ToolbarPlayModePlayButton"
            };
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(
                new[] {
                UnmaskedView.CreateInstanceForGUIView<Toolbar>(new[] { playButtonContrlSelector }),
                UnmaskedView.CreateInstanceForGUIView<AppStatusBar>()
            }
                );
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // only the play button should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get)
                {
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                    Assert.AreEqual(1, rects.Count);
                    var viewPosition = view.position;
                    var controlRect = rects[0];
                    Assert.Greater(controlRect.xMin, viewPosition.xMin);
                    Assert.Greater(controlRect.yMin, viewPosition.yMin);
                    Assert.Less(controlRect.xMax, viewPosition.xMax);
                    Assert.Less(controlRect.yMax, viewPosition.yMax);
                }
                else
                {
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                }
            }
        }

        [Test]
        [Ignore("TODO Fix")]
        public void ApplyHighlighting_ToAllUnmaskedWindowsAndViews_WhenMaskingSettingsOnlySpecifyEntireWindowsAndViews()
        {
            firstPage.m_Paragraphs[0].maskingSettings.SetUnmaskedViews(
                new[] {
                UnmaskedView.CreateInstanceForGUIView<Toolbar>(),
                UnmaskedView.CreateInstanceForGUIView<AppStatusBar>()
            }
                );
            firstPage.m_Paragraphs[0].maskingSettings.enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // both the toolbar and status bar should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get || view is AppStatusBar)
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                else
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
            }
        }

    }
}
