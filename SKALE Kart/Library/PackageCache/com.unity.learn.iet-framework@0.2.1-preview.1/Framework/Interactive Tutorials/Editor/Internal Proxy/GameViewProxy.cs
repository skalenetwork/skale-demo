using UnityEditor;

namespace Unity.InteractiveTutorials
{
    public class GameViewProxy : EditorWindow
    {
        public static bool maximizeOnPlay
        {
            get { return GetWindow<GameView>().maximizeOnPlay; }
            set { GetWindow<GameView>().maximizeOnPlay = value; }
        }
    }
}
