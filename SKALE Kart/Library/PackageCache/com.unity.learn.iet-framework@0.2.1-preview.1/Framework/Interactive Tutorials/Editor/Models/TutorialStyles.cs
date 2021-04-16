using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    class AllTutorialStyles
    {
        // TODO Use this
        //static void FindStyle(string name, ref GUIStyle style)
        //{
        //    style = GUI.skin.FindStyle(name);
        //    Debug.Assert(style != null, $"Syle '{name} not found.");
        //}

        static AllTutorialStyles()
        {
            headerLabel = GUI.skin.FindStyle(s_HeaderStyleName);
            instructionLabel = GUI.skin.FindStyle(s_InstructionLabelStyleName);
            line = GUI.skin.FindStyle(s_LineStyleName);
            listPrefix = GUI.skin.FindStyle(s_ListPrefixStyleName);
            list = GUI.skin.FindStyle(s_ListStyleName);
            progressLabel = GUI.skin.FindStyle(s_ProgressLabelStyle);
            sectionTitleLabel = GUI.skin.FindStyle(s_SectionTitleLabelStyle);
            theInBetweenText = GUI.skin.FindStyle(s_TheInBetweenTextStyle);
            narrativeStyle = GUI.skin.FindStyle(s_Narrative);
            switchTutorialStyle = GUI.skin.FindStyle(s_SwitchTutorialStyleName);
            imageStyle = GUI.skin.FindStyle(s_ImageStyle);
            videoStyle = GUI.skin.FindStyle(s_VideoStyle);

            sectionTitleBackground = GUI.skin.FindStyle(s_SectionTitleBackground);
            topBarBackground = GUI.skin.FindStyle(s_TopBarBackground);
            headerBGStyle = GUI.skin.FindStyle(s_HeaderStyleBG);
            listBGStyle = GUI.skin.FindStyle(s_ListBG);
            theInBetweenTextNotActiveOrCompleted = GUI.skin.FindStyle(s_TheInBetweenTextStyleNotActiveOrCompleted);
            completedElementBackground = GUI.skin.FindStyle(s_CompletedElementBackgroundStyle);
            activeElementBackground = GUI.skin.FindStyle(s_ActiveElementBackgroundStyle);
            inActiveElementBackground = GUI.skin.FindStyle(s_InActiveElementBackgroundStyle);
            darkBackground = GUI.skin.FindStyle(s_DarkBackground);
            bgTheInBetweenText = GUI.skin.FindStyle(s_BGTheInBetweenText);
            background = GUI.skin.FindStyle(s_Background);

            footer = GUI.skin.FindStyle(s_Footer);
            paginationLabel = GUI.skin.FindStyle(s_PaginationLabel);
            nextButton = GUI.skin.FindStyle(s_NextButton);
            nextButtonDisabled = GUI.skin.FindStyle(s_NextButtonDisabled);
            backButton = GUI.skin.FindStyle(s_BackButton);
            backButtonDisabled = GUI.skin.FindStyle(s_BackButtonDisabled);
            progressBar = GUI.skin.FindStyle(s_ProgressBar);

            //iconButtonBack = GUI.skin.FindStyle(s_IconButtonBack);
            iconButtonReset = GUI.skin.FindStyle(s_IconButtonReset);
            iconButtonHome = GUI.skin.FindStyle(s_IconButtonHome);
            iconButtonClose = GUI.skin.FindStyle(s_IconButtonClose);
            instructionLabelIconCompleted = GUI.skin.FindStyle(s_InstructionLabelIconStyleCompleted);
            instructionLabelIconNotCompleted = GUI.skin.FindStyle(s_InstructionLabelIconStyleNotCompleted);

            tutorialCard = GUI.skin.FindStyle(s_TutorialCard);
            linkCard = GUI.skin.FindStyle(s_LinkCard);

            welcomeDialogButton = GUI.skin.FindStyle(s_WelcomeDialogButton);

            tooltip = GUI.skin.FindStyle(s_Tooltip);
        }

        private static readonly string s_InstructionLabelStyleName = "Instruction Label";
        private static readonly string s_Narrative = "NarrativeStyle";
        private static readonly string s_SwitchTutorialStyleName = "SwitchTutorialStyle";
        private static readonly string s_ImageStyle = "ImageStyle";
        private static readonly string s_VideoStyle = "VideoStyle";
        private static readonly string s_HeaderStyleName = "Header";
        private static readonly string s_LineStyleName = "Line";
        private static readonly string s_ListStyleName = "List";
        private static readonly string s_ListPrefixStyleName = "ListPrefix";
        private static readonly string s_ProgressLabelStyle = "Progress Label";
        private static readonly string s_SectionTitleLabelStyle = "Section Title Label";
        private static readonly string s_TheInBetweenTextStyle = "TheInBetweenText";

        private static readonly string s_Background = "Background";
        private static readonly string s_HeaderStyleBG = "SectionTitleBackground";
        private static readonly string s_ListBG = "ListBackground";
        private static readonly string s_SectionTitleBackground = "SectionTitleBackground";
        private static readonly string s_TopBarBackground = "TopBarBackground";
        private static readonly string s_DarkBackground = "DarkBackground";
        private static readonly string s_CompletedElementBackgroundStyle = "CompletedElementBackground";
        private static readonly string s_ActiveElementBackgroundStyle = "ActiveElementBackground";
        private static readonly string s_InActiveElementBackgroundStyle = "InActiveElementBackground";
        private static readonly string s_BGTheInBetweenText = "BGTheInBetweenText";
        private static readonly string s_TheInBetweenTextStyleNotActiveOrCompleted = "BGTheInBetweenTextNotActiveOrCompleted";

        private static readonly string s_Footer = "Footer";
        private static readonly string s_PaginationLabel = "PaginationLabel";
        private static readonly string s_NextButton = "NextButton";
        private static readonly string s_NextButtonDisabled = "NextButtonDisabled";
        private static readonly string s_BackButton = "BackButton";
        private static readonly string s_BackButtonDisabled = "BackButtonDisabled";
        private static readonly string s_ProgressBar = "ProgressBar";

        private static readonly string s_InstructionLabelIconStyleNotCompleted = "InstructionLabelIconNotCompleted";
        private static readonly string s_InstructionLabelIconStyleCompleted = "InstructionLabelIconCompleted";
        //private static readonly string s_IconButtonBack = "IconButtonBack";
        private static readonly string s_IconButtonReset = "IconButtonReset";
        private static readonly string s_IconButtonHome = "IconButtonHome";
        private static readonly string s_IconButtonClose = "IconButtonClose";

        private static readonly string s_TutorialCard = "TutorialCard";
        private static readonly string s_LinkCard = "LinkCard";

        private static readonly string s_WelcomeDialogButton = "WelcomeDialogButton";

        private static readonly string s_Tooltip = "Tooltip";

        public static GUIStyle narrativeStyle;
        public static GUIStyle switchTutorialStyle;
        public static GUIStyle imageStyle;
        public static GUIStyle videoStyle;
        public static GUIStyle theInBetweenText;
        public static GUIStyle headerLabel;
        public static GUIStyle instructionLabel;
        public static GUIStyle line;
        public static GUIStyle list;
        public static GUIStyle listPrefix;
        public static GUIStyle progressLabel;
        public static GUIStyle sectionTitleLabel;

        public static GUIStyle footer;
        public static GUIStyle paginationLabel;
        public static GUIStyle nextButton;
        public static GUIStyle nextButtonDisabled;
        public static GUIStyle backButton;
        public static GUIStyle backButtonDisabled;
        public static GUIStyle progressBar;

        public static GUIStyle background;
        public static GUIStyle sectionTitleBackground;
        public static GUIStyle topBarBackground;
        public static GUIStyle bgTheInBetweenText;
        public static GUIStyle completedElementBackground;
        public static GUIStyle activeElementBackground;
        public static GUIStyle inActiveElementBackground;
        public static GUIStyle darkBackground;
        public static GUIStyle theInBetweenTextNotActiveOrCompleted;
        public static GUIStyle headerBGStyle;
        public static GUIStyle listBGStyle;

        public static GUIStyle instructionLabelIconNotCompleted;
        public static GUIStyle instructionLabelIconCompleted;
        //public static GUIStyle iconButtonBack;
        public static GUIStyle iconButtonReset;
        public static GUIStyle iconButtonHome;
        public static GUIStyle iconButtonClose;

        public static GUIStyle tutorialCard;
        public static GUIStyle linkCard;

        public static GUIStyle welcomeDialogButton;

        public static GUIStyle tooltip;
    }

    class TutorialStyles : ScriptableObject
    {
        public static void DisplayErrorMessage(string fileName)
        {
            EditorGUILayout.HelpBox(
                string.Format(
                    "No styles assigned. Did you forget to assign a default one in the inspector for {0}? Assign one and reopen this window.",
                    fileName
                    ),
                MessageType.Error
                );
        }

        public GUISkin skin { get { return m_Skin; } }
        [SerializeField]
        GUISkin m_Skin = null;

        public string orderedListDelimiter { get { return m_OrderedListDelimiter; } }
        [SerializeField]
        string m_OrderedListDelimiter = ".";

        public string unorderedListBullet { get { return m_UnorderedListBullet; } }
        [SerializeField]
        string m_UnorderedListBullet = "\u2022";

        public Color maskingColor { get { return m_MaskingColor; } }
        [SerializeField]
        private Color m_MaskingColor = new Color32(0, 40, 53, 204);

        public Color highlightColor { get { return m_HighlightColor; } }
        [SerializeField]
        private Color m_HighlightColor = new Color32(0, 198, 223, 255);

        public float highlightThickness { get { return m_HighlightThickness; } }

        public Color blockedInteractionColor { get { return m_BlockedInteractionColor; } }
        [SerializeField]
        private Color m_BlockedInteractionColor = new Color(1, 1, 1, 0.5f);

        [SerializeField, Range(0f, 10f)]
        private float m_HighlightThickness = 3f;

        [SerializeField, Range(0f, 10f)]
        private float m_HighlightAnimationSpeed = 1.5f;

        [SerializeField, Range(0f, 10f)]
        private float m_HighlightAnimationDelay = 5f;

        public Color TextColorMainDarkSkin = Color.white;
        public Color TextColorMainLightSkin = Color.black;

        public Color TextColorSecondaryDarkSkin = new Color(0.66f, 0.66f, 0.66f);
        public Color TextColorSecondaryLightSkin = new Color(0.33f, 0.33f, 0.33f);

        void OnEnable()
        {
            MaskingManager.highlightAnimationSpeed = m_HighlightAnimationSpeed;
            MaskingManager.highlightAnimationDelay = m_HighlightAnimationDelay;
        }

        void OnValidate()
        {
            MaskingManager.highlightAnimationSpeed = m_HighlightAnimationSpeed;
            MaskingManager.highlightAnimationDelay = m_HighlightAnimationDelay;
        }
    }
}
