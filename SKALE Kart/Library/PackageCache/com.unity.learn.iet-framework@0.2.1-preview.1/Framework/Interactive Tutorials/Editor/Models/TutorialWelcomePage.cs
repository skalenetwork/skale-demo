using System.Collections.Generic;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    class TutorialWelcomePage : ScriptableObject
    {
        public Texture icon { get { return m_Icon; } }
        [SerializeField]
        private Texture m_Icon = null;

        public string title { get { return m_Title; } }
        [SerializeField]
        private string m_Title = null;

        public IEnumerable<TutorialParagraph> paragraphs { get { return m_Paragraphs; } }
        [SerializeField]
        private TutorialParagraphCollection m_Paragraphs = new TutorialParagraphCollection();

        public string startButtonLabel { get { return m_StartButtonLabel; } }
        [SerializeField]
        public string m_StartButtonLabel = "Start";

        public string finishButtonLabel { get { return m_FinishButtonLabel; } }
        [SerializeField]
        public string m_FinishButtonLabel = "Go to the next tutorial";
    }
}
