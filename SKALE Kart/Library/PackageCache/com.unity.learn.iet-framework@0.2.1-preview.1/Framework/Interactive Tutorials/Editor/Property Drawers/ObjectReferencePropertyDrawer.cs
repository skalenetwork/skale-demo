using Unity.InteractiveTutorials;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

[CustomPropertyDrawer(typeof(ObjectReference))]
public class ObjectReferencePropertyDrawer : PropertyDrawer
{
    const string k_SceneObjectReferencePath = "m_SceneObjectReference";
    const string k_FutureObjectReferencePath = "m_FutureObjectReference";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var sceneObjectReferenceProperty = property.FindPropertyRelative(k_SceneObjectReferencePath);
        var futureObjectReferenceProperty = property.FindPropertyRelative(k_FutureObjectReferencePath);

        var origColor = GUI.color;

        UnityObject obj;
        SceneObjectReference sceneObjectReference = null;

        if (futureObjectReferenceProperty.objectReferenceValue != null)
        {
            label.text = "(Future) " + label.text;
            GUI.color = Color.cyan;

            obj = futureObjectReferenceProperty.objectReferenceValue;
        }
        else
        {
            sceneObjectReference = new SceneObjectReference(sceneObjectReferenceProperty);

            if (!sceneObjectReference.ReferenceResolved)
            {
                label.text = "(Not resolved) " + label.text;
                GUI.color = Color.red;
            }

            obj = sceneObjectReference.ReferencedObject;
            if (!sceneObjectReference.ReferenceResolved)
            {
                obj = sceneObjectReference.ReferenceScene;
            }
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);
        GUI.color = origColor;

        EditorGUI.BeginChangeCheck();
        var newObj = EditorGUI.ObjectField(position, obj, typeof(Object), true);
        if (EditorGUI.EndChangeCheck())
        {
            if (newObj is FutureObjectReference)
                futureObjectReferenceProperty.objectReferenceValue = newObj;
            else
            {
                futureObjectReferenceProperty.objectReferenceValue = null;

                if (sceneObjectReference == null)
                    sceneObjectReference = new SceneObjectReference(sceneObjectReferenceProperty);
                sceneObjectReference.Update(newObj);
            }
        }

        EditorGUI.EndProperty();
    }
}
