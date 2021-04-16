using System;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    enum SceneViewCameraMode { SceneView2D, SceneView3D };
    enum SceneViewFocusMode { Manual, FrameObject };

    [Serializable]
    class SceneViewCameraSettings
    {
        public SceneViewCameraMode cameraMode { get { return m_CameraMode; } }
        [SerializeField]
        SceneViewCameraMode m_CameraMode = SceneViewCameraMode.SceneView2D;

        public SceneViewFocusMode focusMode { get { return m_FocusMode; } }
        [SerializeField]
        SceneViewFocusMode m_FocusMode = SceneViewFocusMode.Manual;

        public bool orthographic { get { return m_Orthographic; } }
        [SerializeField]
        bool m_Orthographic = false;

        public float size { get { return m_Size; } }
        [SerializeField]
        float m_Size = default;

        public Vector3 pivot { get { return m_Pivot; } }
        [SerializeField]
        Vector3 m_Pivot = default ;

        public Quaternion rotation { get { return m_Rotation; } }
        [SerializeField]
        Quaternion m_Rotation = default;

        public SceneObjectReference frameObject { get { return m_FrameObject; } }
        [SerializeField]
        SceneObjectReference m_FrameObject = null;

        public bool enabled { get { return m_Enabled; } }
        [SerializeField]
        bool m_Enabled = false;

        public void Apply()
        {
            var sceneView = EditorWindow.GetWindow<SceneView>(null, false);
            sceneView.in2DMode = (cameraMode == SceneViewCameraMode.SceneView2D);
            switch (focusMode)
            {
                case SceneViewFocusMode.FrameObject:
                    GameObject go = frameObject.ReferencedObjectAsGameObject;
                    if (go == null)
                        throw new InvalidOperationException("Error looking up frame object");
                    sceneView.Frame(GameObjectProxy.CalculateBounds(go), true);
                    break;
                case SceneViewFocusMode.Manual:
                    sceneView.LookAt(pivot, rotation, size, orthographic, false);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Focus mode {0} not supported", focusMode));
            }
        }
    }
}
