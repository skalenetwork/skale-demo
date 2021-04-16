using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public static class TutorialWindowMenuItem
    {
        public const string Menu = "Tutorials";
        public const string MenuPath = Menu + "/";
        public const string Item = "Show Tutorials";

        [MenuItem(MenuPath + Item)]
        static void OpenTutorialWindow()
        {
            TutorialWindow.CreateWindow();
        }
    }
}
