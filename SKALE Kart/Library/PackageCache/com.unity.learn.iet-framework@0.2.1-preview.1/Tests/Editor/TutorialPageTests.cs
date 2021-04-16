using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using NUnit.Framework;
using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials.Tests
{
    public class TutorialPageTests
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

        TutorialPage m_Page;
        MockCriterion m_Criterion;

        [SetUp]
        public void SetUp()
        {
            m_Page = ScriptableObject.CreateInstance<TutorialPage>();

            var instructionParagraph = new TutorialParagraph();
            instructionParagraph.m_Type = ParagraphType.Instruction;

            m_Criterion = ScriptableObject.CreateInstance<MockCriterion>();
            var tc = new TypedCriterion(new SerializedType(typeof(MockCriterion)), m_Criterion);

            instructionParagraph.m_Criteria = new TypedCriterionCollection(new[] {tc});

            m_Page.m_Paragraphs = new TutorialParagraphCollection(new[] {instructionParagraph});

            m_Page.Initiate();
        }

        [TearDown]
        public void TearDown()
        {
            if (m_Page == null)
                return;

            foreach (var paragraph in m_Page.paragraphs)
            {
                foreach (var criterion in paragraph.criteria)
                {
                    if (criterion != null && criterion.criterion != null)
                        UnityObject.DestroyImmediate(criterion.criterion);
                }
            }

            UnityObject.DestroyImmediate(m_Page);
        }

        [Test]
        public void PageMarkedIncomplete_WhenACriterionIsInvalidated()
        {
            Assert.IsFalse(m_Page.allCriteriaAreSatisfied);
            m_Criterion.Complete(true);
            Assert.IsTrue(m_Page.allCriteriaAreSatisfied);
            m_Criterion.Complete(false);
            Assert.IsFalse(m_Page.allCriteriaAreSatisfied);
        }

        [Test]
        public void PageRemainsComplete_IfCriterionIsInvalidated_AfterAdvancingToNextPage()
        {
            Assert.IsFalse(m_Page.allCriteriaAreSatisfied);
            m_Criterion.Complete(true);
            Assert.IsTrue(m_Page.allCriteriaAreSatisfied);

            m_Page.OnPageCompleted();
            Assert.IsTrue(m_Page.hasMovedToNextPage);

            m_Criterion.Complete(false);
            Assert.IsTrue(m_Page.allCriteriaAreSatisfied);
        }

        [Test]
        public void PagePlaysCompletionSound_WheneverActiveInstructionCriterionIsCompleted()
        {
            var playedSound = 0;
            m_Page.playedCompletionSound += page => ++ playedSound;

            m_Criterion.Complete(true);

            Assert.AreEqual(1, playedSound, "Did not play sound when criterion was completed.");

            m_Criterion.Complete(false);
            m_Criterion.Complete(true);

            Assert.AreEqual(2, playedSound, "Did not play sound when criterion was completed after being invalidated.");
        }

        [Test]
        public void PageDoesNotPlayCompletionSound_WhenCompletedCriterionIsNotActiveInstruction()
        {
            MockCriterion secondCriterion = null;
            try
            {
                secondCriterion = ScriptableObject.CreateInstance<MockCriterion>();
                var secondInstruction = new TutorialParagraph
                {
                    m_Type = ParagraphType.Instruction,
                    m_Criteria = new TypedCriterionCollection(
                            new[] { new TypedCriterion(new SerializedType(typeof(MockCriterion)), secondCriterion) }
                            )
                };
                m_Page.m_Paragraphs =
                    new TutorialParagraphCollection(new[] { m_Page.paragraphs.First(), secondInstruction });

                var playedSound = false;
                m_Page.playedCompletionSound += page => playedSound = true;

                secondCriterion.Complete(true);
                Assert.IsFalse(playedSound, "Played sound for second step when first step was not yet completed.");
            }
            finally
            {
                if (secondCriterion != null)
                    UnityObject.DestroyImmediate(secondCriterion);
            }
        }
    }
}
