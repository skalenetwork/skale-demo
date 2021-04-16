using UnityEditor.Connect;

namespace Unity.InteractiveTutorials
{
    public static class UnityConnectProxy
    {
        public static string GetUserId()
        {
            return UnityConnect.instance.GetUserId();
        }

        public static string GetAccessToken()
        {
            return UnityConnect.instance.GetAccessToken();
        }
    }
}
