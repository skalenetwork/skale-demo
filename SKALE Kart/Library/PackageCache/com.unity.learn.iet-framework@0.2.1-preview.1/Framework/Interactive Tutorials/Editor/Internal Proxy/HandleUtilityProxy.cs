using UnityEditor;
using UnityEngine;

public static class HandleUtilityProxy
{
    public static GameObject FindSelectionBase(GameObject gameObject)
    {
        return HandleUtility.FindSelectionBase(gameObject);
    }
}
