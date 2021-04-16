using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public static class HomeWindowProxy
    {
        public static void ShowTutorials()
        {
            HomeWindow.Show(HomeWindow.HomeMode.Tutorial);
        }
    }
}
