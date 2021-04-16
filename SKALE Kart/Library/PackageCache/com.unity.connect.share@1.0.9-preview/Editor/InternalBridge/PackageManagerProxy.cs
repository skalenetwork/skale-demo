using UnityEditor;
using UnityEditorInternal;

public static class PackageManagerProxy
{
    // Returns also hidden packages
    public static UnityEditor.PackageManager.PackageInfo[] GetAllVisiblePackages()
    {
        return PackageManagerUtilityInternal.GetAllVisiblePackages(
#if UNITY_2019_2_OR_NEWER
            skipHiddenPackages:false
#endif
        );
    }

    // As Application.identifier cannot be trusted (it can return empty on WebGL, for example)
    // read the value directly from the ProjectSettings.
    // TODO Move to a dedicated class
    public static string GetApplicationIdentifier()
    {
        var projectSettings = InternalEditorUtility.LoadSerializedFileAndForget("ProjectSettings/ProjectSettings.asset");
        using(var so = new SerializedObject(projectSettings[0]))
            return so.FindProperty("applicationIdentifier.Array.data[0].second").stringValue;
    }
}
