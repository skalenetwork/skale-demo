using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor.Connect;

namespace Unity.InteractiveTutorials
{
    [InitializeOnLoad]
    public static class GenesisHelper
    {
        static readonly string prodHost = @"https://api.unity.com";
        static readonly string stagingHost = @"https://api-staging.unity.com";

        private static List<KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>> m_Requests = new List<KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>>();

        private static string HostAddress
        {
            get { return (IsStagingEnv() ? stagingHost : prodHost); }
        }

        private static bool IsStagingEnv()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            for (int i = 0; i < commandLineArgs.Length; i++)
            {
                if (commandLineArgs[i] == "-cloudEnvironment")
                {
                    if (i + 1 < commandLineArgs.Length && commandLineArgs[i + 1] == "staging")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static GenesisHelper()
        {
            EditorApplication.update += WebRequestProcessor;
        }

        public static void LogTutorialStarted(string lessonId)
        {
            if (!IsLessonIdValid(lessonId))
            {
                return;
            }
            GetTutorial(lessonId, (list) =>
                {
                    var lesson = list.FirstOrDefault(s => s.lessonId == lessonId);
                    if (lesson == null || string.IsNullOrEmpty(lesson.status.Trim()))
                    {
                        // The fact no entries exist means the user never opened (started) this tutorial
                        LogTutorialStatusUpdate(lessonId, TutorialProgressStatus.Status.Started.ToString());
                    }
                });
        }

        public static void LogTutorialEnded(string lessonId)
        {
            if (!IsLessonIdValid(lessonId))
            {
                return;
            }
            GetTutorial(lessonId, (list) =>
                {
                    var lesson = list.FirstOrDefault(s => s.lessonId == lessonId);
                    if (lesson == null || lesson.status != TutorialProgressStatus.Status.Finished.ToString())
                    {
                        // We only set the status to Finished if:
                        // there was no entry (which should not happen)
                        // the current status is Started
                        LogTutorialStatusUpdate(lessonId, TutorialProgressStatus.Status.Finished.ToString());
                    }
                });
        }

        private static bool IsLessonIdValid(string lessonId)
        {
            if (string.IsNullOrEmpty(lessonId.Trim()))
            {
                LogWarningOnlyInAuthoringMode("LessonId is not set. You can set LessonId on the tutorial asset");
                return false;
            }
            return true;
        }

        private static void LogWarningOnlyInAuthoringMode(string message)
        {
            // We don't want to spam users with warning messages
            // but we want to catch them while creating tutorials
            if (ProjectMode.IsAuthoringMode())
                Debug.LogWarning(message);
        }

        public static void PrintAllTutorials()
        {
            GetAllTutorials((tutorials) =>
                {
                    var result = "";
                    foreach (var tutorial in tutorials)
                    {
                        result += tutorial.lessonId + ": " + tutorial.status + "\n";
                    }
                    Debug.Log(result);
                });
        }

        public static void GetAllTutorials(Action<List<TutorialProgressStatus>> action)
        {
            GetTutorial(null, action);
        }

        private static bool IsRequestSuccess(UnityWebRequest unityWebRequest)
        {
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                LogWarningOnlyInAuthoringMode(unityWebRequest.error);
                return false;
            }
            return true;
        }

        private static void GetTutorial(string lessonId, Action<List<TutorialProgressStatus>> action)
        {
            var userId = UnityConnectProxy.GetUserId();
            var getLink = @"/v1/users/" + userId + @"/lessons";
            var address = HostAddress + getLink;
            var req = MakeGetLessonsRequest(address, lessonId);
            SendWebRequest(req, (UnityWebRequest r) =>
                {
                    if (!IsRequestSuccess(r))
                    {
                        return;
                    }
                    var lessonResponses = TutorialProgressStatus.ParseResponses(r.downloadHandler.text);
                    action(lessonResponses);
                });
        }

        public static void LogTutorialStatusUpdate(string lessonId, string lessonStatus)
        {
            var userId = UnityConnectProxy.GetUserId();
            var getLink = @"/v1/users/" + userId + @"/lessons";
            var address = HostAddress + getLink;

            var jsonData = RegisterLessonRequest.GetJSONString(lessonStatus, userId, lessonId);
            var req = UnityWebRequest.Post(address, jsonData);
            var data = System.Text.Encoding.UTF8.GetBytes(jsonData);
            req.uploadHandler = new UploadHandlerRaw(data);

            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + UnityConnectProxy.GetAccessToken());

            SendWebRequest(req, r =>
                {
                    if (!IsRequestSuccess(r))
                    {
                        return;
                    }
                });
        }

        private static void SendWebRequest(UnityWebRequest request, Action<UnityWebRequest> onFinished)
        {
            var pair = new KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>(request.SendWebRequest(), onFinished);
            m_Requests.Add(pair);
        }

        private static void WebRequestProcessor()
        {
            if (!m_Requests.Any())
                return;

            for (int i = 0; i < m_Requests.Count; i++)
            {
                var request = m_Requests[i].Key;
                if (!request.isDone)
                    continue;
                var callback = m_Requests[i].Value;
                m_Requests.RemoveAt(i);
                callback(request.webRequest);
                break;
            }
        }

        private static UnityWebRequest MakeGetLessonsRequest(string address, string lessonId)
        {
            if (!string.IsNullOrEmpty(lessonId))
                address += "?lessonId=" + lessonId;

            var req = UnityWebRequest.Post(address, new WWWForm());
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + UnityConnectProxy.GetAccessToken());
            req.method = "GET";
            return req;
        }

        [Serializable]
        public class TutorialProgressStatus
        {
            public string lessonId;
            public string status; // TODO make an enum instead

            public enum Status
            {
                Started,
                Finished
            }

            [Serializable]
            class Wrapper
            {
                public List<TutorialProgressStatus> statuses = null;
            }

            public static List<TutorialProgressStatus> ParseResponses(string respText)
            {
                var builder = new StringBuilder(12 + respText.Length + 1);
                builder.Append("{\"statuses\":");
                builder.Append(respText);
                builder.Append("}");
                var wrapper = JsonUtility.FromJson<Wrapper>(builder.ToString());
                return wrapper.statuses;
            }
        }

        private class RegisterLessonRequest
        {
            public string status;
            public string userId;
            public string lessonId;

            public static string GetJSONString(string status, string userId, string lessonId)
            {
                var r = new RegisterLessonRequest();
                r.status = status;
                r.userId = userId;
                r.lessonId = lessonId;
                return JsonUtility.ToJson(r);
            }
        }
    }
}
