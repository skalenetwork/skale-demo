using System.Collections;
using Unity.Connect.Share.Editor.store;
using Unity.Connect.Share.UIWidgets.Redux;
using Unity.UIWidgets.async;
using Unity.UIWidgets.editor;
using Unity.UIWidgets.material;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.ui;
using UnityEditor;
using UnityEngine;

namespace Unity.Connect.Share.Editor
{
    public class ConnectShareEditorWindow : UIWidgetsEditorWindow
    {
        [MenuItem("Window/Share WebGL Game")]
        static void OpenShareWindow()
        {
            string token = UnityConnectSession.instance.GetAccessToken();
            if (token.Length == 0)
            {
                StoreFactory.get().Dispatch(new NotLoginAction());
            }

            var window = GetWindow<ConnectShareEditorWindow>();
            window.titleContent.text = "Share";
            window.minSize = new Vector2(600f, 419f);
            window.maxSize = window.minSize;
            window.Show();
        }

        public static UIWidgetsCoroutine StartCoroutine(IEnumerator coroutine)
        {
            var window = GetWindow<ConnectShareEditorWindow>(false, "", false);
            return window.window.startCoroutine(coroutine);
        }

        protected override Widget createWidget()
        {
            return new MaterialApp(
                home: new StoreProvider<AppState>(StoreFactory.get(), new ShareReduxWidget())
            );
        }

        public void Awake()
        {
            FontManager.instance.addFont(Resources.Load<Font>("MaterialIcons-Regular"), "Material Icons");
        }

        protected override void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            base.OnEnable();
        }

        public void OnBeforeAssemblyReload()
        {
            SaveStateToSessionState();
        }

        protected override void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            StoreFactory.get().Dispatch(new DestroyAction());
        }

        static void SaveStateToSessionState()
        {
            var shareState = StoreFactory.get().state.shareState;
            SessionState.SetString(typeof(ConnectShareEditorWindow).Name, EditorJsonUtility.ToJson(shareState));
        }
    }
}
