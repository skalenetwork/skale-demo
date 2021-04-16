using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace KartGame.Timeline
{
    /// <summary>
    /// A MonoBehaviour that has UnityEvents for when a timeline starts and ends which are triggered automatically.
    /// </summary>
    public class DirectorTrigger : MonoBehaviour
    {
        /// <summary>
        /// Contains an UnityEvent and a time at which it is triggered.
        /// </summary>
        [Serializable]
        public class TimelineEvent
        {
            [Tooltip("The time through the timeline at which the event will trigger.")]
            public float time;
            public UnityEvent timedEvent;

            bool m_Triggered;

            /// <summary>
            /// Call this to check whether the event should be triggered yet and if so, trigger it.
            /// </summary>
            /// <param name="currentTime">The current time of the timeline playing.</param>
            public void CheckAndTriggerEvent (float currentTime)
            {
                if (!m_Triggered && currentTime >= time)
                {
                    timedEvent.Invoke ();
                    m_Triggered = true;
                }
            }

            /// <summary>
            /// Allows for the event to be triggered again.  This should be called at the end of a timeline if it loops or otherwise needs to be called again.
            /// </summary>
            public void Reset ()
            {
                m_Triggered = false;
            }
        }
        
        
        [Tooltip("The playable director to be played.")]
        public PlayableDirector director;
        [Tooltip("These events will be triggered at their corresponding time through the duration of the timeline.")]
        public TimelineEvent[] events;

        bool m_StartTimer;
        float m_Timer;

        void Awake ()
        {
            ResetTrigger ();
        }

        void Update ()
        {
            if (m_StartTimer)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    events[i].CheckAndTriggerEvent (m_Timer);
                }

                m_Timer += Time.deltaTime;
            }
        }
        
        /// <summary>
        /// Starts the Playable Director and triggers the events over its duration.
        /// </summary>
        public void TriggerDirector ()
        {
            director.Play();
            m_StartTimer = true;
        }

        /// <summary>
        /// Stops the Playable Director and resets all the events.
        /// </summary>
        public void ResetTrigger ()
        {
            director.Stop ();
            m_StartTimer = false;
            m_Timer = 0f;

            for (int i = 0; i < events.Length; i++)
            {
                events[i].Reset ();
            }
        }
    }
}