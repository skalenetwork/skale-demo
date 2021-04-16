using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Unity.Connect.Share.UIWidgets.Redux;
using Unity.UIWidgets.async;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity.Connect.Share.Editor.store
{
    public class ShareMiddleware
    {
        private static UnityWebRequest uploadRequest;
        private const string webglSharingFile = "webgl_sharing";
        private const string zipName = "connectwebgl.zip";
        private const string thumbnail = "thumbnail.png";
        private const string uploadEndpoint = "/api/webgl/upload";
        private const string queryProgressEndpoint = "/api/webgl/progress";
        private const int ZipFileLimitBytes = 100 * 1024 * 1024;

        public static Middleware<AppState> Create() {
            return (store) => (next) => (action) => {
                var result = next(action);

                switch (action)
                {
                    case ShareStartAction shared:
                        ZipAndShare(shared.title, store);
                        break;    
                    case UploadStartAction upload:
                        Upload(store);
                        break;
                    case QueryProgressAction query:
                        CheckProgress(store, query.key);
                        break;
                    case StopUploadAction stopUpload:
                        StopUploadAction();
                        break;
                    case NotLoginAction login:
                        CheckLoginStatus(store);
                        break;
                }
                return result;
            };
        }

        private static void ZipAndShare(string title, Store<AppState> store)
        {
            store.Dispatch(new TitleChangeAction { title = title});
            
            var buildOutputDir = store.state.shareState.buildOutputDir;
            if (string.IsNullOrEmpty(buildOutputDir) || !Directory.Exists(buildOutputDir)) {
                store.Dispatch(new OnErrorAction { errorMsg = "Please build project first!" });
                return;
            }
            
            if (Zip(store)) {
                store.Dispatch(new UploadStartAction());
            }
        }

        private static bool Zip(Store<AppState> store)
        {
            var projectDir = Directory.GetParent(Application.dataPath).FullName;
            var buildOutputDir = store.state.shareState.buildOutputDir;
            var destPath = Path.Combine(projectDir, zipName);
            
            File.Delete(destPath);

            CopyThumbnail(store);

            ZipFile.CreateFromDirectory(buildOutputDir, destPath);
            var fileInfo = new System.IO.FileInfo(destPath);

            if (fileInfo.Length > ZipFileLimitBytes) {
                store.Dispatch(new OnErrorAction{errorMsg = $"Max. allowed WebGL game .zip size is {Utils.FormatBytes(ZipFileLimitBytes)}."});
                return false;
            } else {
                store.Dispatch(new ZipPathChangeAction{ zipPath = destPath });
                return true;
            } 
        }

        private static void CopyThumbnail(Store<AppState> store)
        {
            var buildOutputDir = store.state.shareState.buildOutputDir;
            var thumbnailDestPath = Path.Combine(buildOutputDir, thumbnail);
            
            File.Delete(thumbnailDestPath);
            
            var thumbnailDir = store.state.shareState.thumbnailDir;

            if (string.IsNullOrEmpty(thumbnailDir)) {
                return;
            }
            
            
            FileUtil.CopyFileOrDirectory(thumbnailDir, thumbnailDestPath);
        }

        private static void Upload(Store<AppState> store)
        {
            var token = UnityConnectSession.instance.GetAccessToken();
            if (token.Length == 0)
            {
                CheckLoginStatus(store);
                return;
            }

            var path = store.state.shareState.zipPath;
            var title = store.state.shareState.title;
            var buildGUID = store.state.shareState.buildGUID;
            
            var baseUrl = getAPIBaseUrl();
            var projectId = GetProjectId();
            var formSections = new List<IMultipartFormSection>();
            
            formSections.Add(new MultipartFormDataSection("title", title));
            
            if (buildGUID.Length > 0) {
                formSections.Add(new MultipartFormDataSection("buildGUID", buildGUID));
            }
            
            if (projectId.Length > 0)
            {
                formSections.Add(new MultipartFormDataSection("projectId", projectId));
            }
            
            

            formSections.Add(new MultipartFormFileSection("file", 
                File.ReadAllBytes(path), Path.GetFileName(path), "application/zip"));
            
            uploadRequest = UnityWebRequest.Post(baseUrl + uploadEndpoint, formSections);
            uploadRequest.SetRequestHeader("Authorization", $"Bearer {token}");
            uploadRequest.SetRequestHeader("X-Requested-With", "XMLHTTPREQUEST");
            
            var op = uploadRequest.SendWebRequest();
            ConnectShareEditorWindow.StartCoroutine(updateProgress(store, uploadRequest));
           
            op.completed += operation =>
            {
                if(uploadRequest.isNetworkError || uploadRequest.isHttpError) {
                    Debug.Log(uploadRequest.error);
                    if (uploadRequest.error != "Request aborted")
                    {
                        store.Dispatch(new OnErrorAction {errorMsg = "Internal server error"});
                    }
                }
                else
                {
                    var response = JsonUtility.FromJson<UploadResponse>(op.webRequest.downloadHandler.text);
                    if (!string.IsNullOrEmpty(response.key))
                    {
                        store.Dispatch(new QueryProgressAction {key = response.key});
                    }
                }
            };
        }

        private static void StopUploadAction()
        {
            if (uploadRequest != null)
            {
                uploadRequest.Abort();
            }
        }

        private static void CheckProgress(Store<AppState> store, string key)
        {
            var token = UnityConnectSession.instance.GetAccessToken();
            if (token.Length == 0)
            {
                CheckLoginStatus(store);
                return;
            }
            
            key = key??store.state.shareState.key;
            var baseUrl = getAPIBaseUrl();
            var uploadRequest = UnityWebRequest.Get($"{baseUrl + queryProgressEndpoint}?key={key}");
            uploadRequest.SetRequestHeader("Authorization", $"Bearer {token}");
            uploadRequest.SetRequestHeader("X-Requested-With", "XMLHTTPREQUEST");
            var op = uploadRequest.SendWebRequest();

            op.completed += operation =>
            {
                if(uploadRequest.isNetworkError || uploadRequest.isHttpError) {
                    Debug.Log(uploadRequest.error);
                }
                else
                {
                    var response = JsonUtility.FromJson<GetProgressResponse>(op.webRequest.downloadHandler.text);
                    
                    store.Dispatch(new QueryProgressResponseAction {response = response});
                    if (response.progress == 100 || !string.IsNullOrEmpty(response.error))
                    {
                        SaveProjectID(response.projectId);
                        return;
                    }
                    
                }
                
                ConnectShareEditorWindow.StartCoroutine(wait(1.5f)).promise.Then((obj) =>
                {
                    store.Dispatch(new QueryProgressAction());
                });
            };
        }
        
        private static void SaveProjectID(string projectId)
        {
            if (projectId.Length == 0)
            {
                return;
            }     
            
            var writer = new StreamWriter(webglSharingFile, false);
            writer.Write(projectId);
            writer.Close();
        }
        
        private static string GetProjectId()
        {
            if (!System.IO.File.Exists(webglSharingFile))
            {
                return "";
            }

            var reader = new StreamReader(webglSharingFile);
            var projectId = reader.ReadLine();
            
            reader.Close();
            return projectId;
        }

        private static IEnumerator updateProgress(Store<AppState> store, UnityWebRequest request)
        {
            while (true)
            {
                if (request.isDone)
                {
                    break;
                }

                int progress = (int) (Mathf.Clamp(request.uploadProgress, 0, 1) * 100);
                store.Dispatch(new UploadProgressAction {progress = progress});
                yield return new UIWidgetsWaitForSeconds(0.5f);
            }

            yield  return null;
        }
        
        private static void CheckLoginStatus(Store<AppState> store)
        {
            var token = UnityConnectSession.instance.GetAccessToken();
            if (token.Length == 0)
            {
                ConnectShareEditorWindow.StartCoroutine(wait(2f)).promise.Then((obj) =>
                {
                    store.Dispatch(new NotLoginAction());
                });
            }
            else
            {
                store.Dispatch(new LoginAction());
            }

        }
        
        private static IEnumerator wait(float seconds)
        {
            yield return new UIWidgetsWaitForSeconds(seconds);
        }

        private static string getAPIBaseUrl()
        {
            var env = UnityConnectSession.instance.GetEnvironment();
            if (env == "staging")
            {
                return "https://connect-staging.unity.com";
            }
            else if (env == "dev")
            {
                return "https://connect-dev.unity.com";
            }

            return "https://connect.unity.com";
        }

    }

    [Serializable]
    public class UploadResponse
    {
        public string key;
    }
    
    
    [Serializable]
    public class GetProgressResponse
    {
        public string projectId;
        public string url;
        public int progress;
        public string error;
    }
    
}
