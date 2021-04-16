using Unity.Connect.Share.UIWidgets.Redux;
using UnityEditor;

namespace Unity.Connect.Share.Editor.store
{
    public class StoreFactory
    {
        private static Store<AppState> _store;

        static void _setupStore()
        {
            var shareState = new ShareState();
            EditorJsonUtility.FromJsonOverwrite(
                SessionState.GetString(typeof(ConnectShareEditorWindow).Name, "{}"),
                shareState
            );

            _store = new Store<AppState>(
                ShareReducer.reducer,
                initialState: new AppState(shareState: shareState),
                middlewares: ShareMiddleware.Create()
            );
        }
        
        public static Store<AppState> get()
        {
            if (_store == null)
            {
                _setupStore();
            }

            return _store;
        }
    }
}