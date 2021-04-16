using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Video;

namespace Unity.InteractiveTutorials
{
    enum ParagraphType
    {
        Narrative,
        Instruction,
        SwitchTutorial,
        UnorderedList,
        OrderedList,
        Icons,
        Image,
        Video,
    }

    enum CompletionType
    {
        CompletedWhenAllAreTrue,
        CompletedWhenAnyIsTrue
    }

    [Serializable]
    class TutorialParagraph
    {
        public ParagraphType type { get { return m_Type; } }
        [SerializeField]
        internal ParagraphType m_Type;

        public string summary { get { return m_Summary; } set { m_Summary = value; } }
        [SerializeField, TextArea(1, 1)]
        string m_Summary = "";

        public string text { get { return m_Text; } set { m_Text = value; } }
        [SerializeField, TextArea(1, 15)]
        string m_Text = "";

        [SerializeField]
        internal string m_TutorialButtonText = "";
        [SerializeField]
        internal Tutorial m_Tutorial;

        public IEnumerable<InlineIcon> icons
        {
            get
            {
                m_Icons.GetItems(m_IconBuffer);
                return m_IconBuffer;
            }
        }
        [SerializeField]
        InlineIconCollection m_Icons = new InlineIconCollection();
        readonly List<InlineIcon> m_IconBuffer = new List<InlineIcon>();

        public Texture2D image { get { return m_Image; } }
        [SerializeField]
        Texture2D m_Image = null;

        public VideoClip video { get { return m_Video; } }
        [SerializeField]
        VideoClip m_Video = null;

        [SerializeField]
        internal CompletionType m_CriteriaCompletion = CompletionType.CompletedWhenAllAreTrue;

        [SerializeField] internal TypedCriterionCollection m_Criteria = new TypedCriterionCollection();
        readonly List<TypedCriterion> m_CriteriaBuffer = new List<TypedCriterion>();

        public IEnumerable<TypedCriterion> criteria
        {
            get
            {
                m_Criteria.GetItems(m_CriteriaBuffer);
                return m_CriteriaBuffer.ToArray();
            }
        }

        public MaskingSettings maskingSettings { get { return m_MaskingSettings; } }
        [SerializeField]
        MaskingSettings m_MaskingSettings = new MaskingSettings();

        public bool completed
        {
            get
            {
                bool allMandatory = m_CriteriaCompletion == CompletionType.CompletedWhenAllAreTrue;
                bool result = allMandatory;

                foreach (var typedCriterion in m_Criteria)
                {
                    var criterion = typedCriterion.criterion;
                    if (criterion != null)
                    {
                        if (!allMandatory && criterion.completed)
                        {
                            result = true;
                            break;
                        }

                        if (allMandatory && !criterion.completed)
                        {
                            result = false;
                            break;
                        }
                    }
                }

                return result;
            }
        }
    }

    [Serializable]
    class TutorialParagraphCollection : CollectionWrapper<TutorialParagraph>
    {
        public TutorialParagraphCollection() : base()
        {
        }

        public TutorialParagraphCollection(IList<TutorialParagraph> items) : base(items)
        {
        }
    }
}
