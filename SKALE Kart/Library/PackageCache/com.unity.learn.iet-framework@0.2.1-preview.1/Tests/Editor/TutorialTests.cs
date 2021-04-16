using NUnit.Framework;
using UnityEngine;

namespace Unity.InteractiveTutorials.Tests
{
    class TutorialTests
    {
        private Tutorial tutorial;

        [Test]
        public void CanMoveToNextPage_WhenAllCriteriaAreSatisfied()
        {
            Tutorial tutorial;
            var c = CreateTutorial(out tutorial);

            tutorial.currentPage.ValidateCriteria();
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());

            c.IsCompleted = true;
            c.UpdateCompletion();

            tutorial.currentPage.ValidateCriteria();
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());
        }

        [Test]
        public void CanNotMoveToNextPage_WhenCriteriaIsNotSatisfiedAnymore()
        {
            Tutorial tutorial;
            var c = CreateTutorial(out tutorial);

            tutorial.currentPage.ValidateCriteria();
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());

            c.IsCompleted = true;
            c.UpdateCompletion();
            tutorial.currentPage.ValidateCriteria();
            c.IsCompleted = false;
            c.UpdateCompletion();
            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());
        }

        [Test]
        public void CanMoveToNextPage_WhenPageWasPreviouslyCompleted()
        {
            Tutorial tutorial;
            var c = CreateTutorial(out tutorial);

            tutorial.currentPage.ValidateCriteria();
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());

            c.IsCompleted = true;
            c.UpdateCompletion();

            tutorial.currentPage.ValidateCriteria();
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.GoToPreviousPage();
            c.IsCompleted = false;
            c.UpdateCompletion();
            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.currentPage.allCriteriaAreSatisfied);
            Assert.IsTrue(tutorial.TryGoToNextPage());

            tutorial.currentPage.ValidateCriteria();
            Assert.IsFalse(tutorial.TryGoToNextPage());
        }

        private static MockCriterion CreateTutorial(out Tutorial tutorial)
        {
            var textParagraph = new TutorialParagraph();
            textParagraph.m_Type = ParagraphType.Instruction;

            var instructionParagraph = new TutorialParagraph();
            instructionParagraph.m_Type = ParagraphType.Instruction;

            var c = ScriptableObject.CreateInstance<MockCriterion>();
            var tc = new TypedCriterion(new SerializedType(typeof(MockCriterion)), c);

            instructionParagraph.m_Criteria = new TypedCriterionCollection(new[] {tc});


            var page1 = ScriptableObject.CreateInstance<TutorialPage>();
            page1.m_Paragraphs = new TutorialParagraphCollection(new[] {textParagraph});

            var page2 = ScriptableObject.CreateInstance<TutorialPage>();
            page2.m_Paragraphs = new TutorialParagraphCollection(new[] {instructionParagraph});

            var page3 = ScriptableObject.CreateInstance<TutorialPage>();
            page3.m_Paragraphs = new TutorialParagraphCollection(new[] {textParagraph});

            tutorial = ScriptableObject.CreateInstance<Tutorial>();

            tutorial.m_Pages = new Tutorial.TutorialPageCollection(new[] {page1, page2, page3});
            return c;
        }

        class MockCriterion : Criterion
        {
            public bool IsCompleted;

            protected override bool EvaluateCompletion()
            {
                return IsCompleted;
            }

            public override bool AutoComplete()
            {
                return true;
            }
        }
    }
}
