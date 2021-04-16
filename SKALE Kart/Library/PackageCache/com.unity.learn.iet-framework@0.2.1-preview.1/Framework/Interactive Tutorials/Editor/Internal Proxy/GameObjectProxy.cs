using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public static class GameObjectProxy
    {
        public static Bounds CalculateBounds(GameObject gameObject)
        {
            return gameObject.CalculateBounds();
        }
    }
}
