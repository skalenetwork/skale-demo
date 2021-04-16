using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unity.InteractiveTutorials
{
    static public class UsabilityAnalyticsProxy
    {
        public static void SendEvent(string eventType, System.DateTime startTime, System.TimeSpan duration, bool isBlocking, object parameters)
        {
            UsabilityAnalytics.SendEvent(eventType, startTime, duration, isBlocking, parameters);
        }
    }
}
