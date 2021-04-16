using System;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    class AutoCompletion
    {
        Tutorial m_Tutorial;

        bool m_Running;
        bool m_Continue;

        const double k_TimeoutDuration = 10f;
        double m_CompletionDeadline;

        public AutoCompletion(Tutorial tutorial)
        {
            m_Tutorial = tutorial;
        }

        public bool running { get { return m_Running; } }

        public void OnEnable()
        {
            if (m_Running)
            {
                if (m_Tutorial.completed)
                    Stop();
                else
                {
                    EditorApplication.update += OnUpdate;
                    Criterion.criterionCompleted += OnCriterionCompleted;
                }
            }
        }

        public void OnDisable()
        {
            if (m_Running)
            {
                EditorApplication.update -= OnUpdate;
                Criterion.criterionCompleted -= OnCriterionCompleted;
            }
        }

        public void Start()
        {
            if (m_Running || m_Tutorial.completed)
                return;

            m_Running = true;
            m_Continue = true;
            m_CompletionDeadline = 0f;

            EditorApplication.update += OnUpdate;
            Criterion.criterionCompleted += OnCriterionCompleted;
        }

        public void Stop()
        {
            if (!m_Running)
                return;

            m_Running = false;

            EditorApplication.update -= OnUpdate;
            Criterion.criterionCompleted -= OnCriterionCompleted;

            if (m_Tutorial.completed)
                Debug.Log("Tutorial was completed automatically.");
            else
                Debug.Log("Tutorial was did not complete automatically.");
        }

        void OnCriterionCompleted(Criterion completedCriterion)
        {
            m_Continue = true;
        }

        void OnUpdate()
        {
            if (m_CompletionDeadline != 0f && EditorApplication.timeSinceStartup > m_CompletionDeadline)
            {
                Debug.LogWarning("Auto completion timed out.");
                Stop();
            }

            if (m_Continue)
                AutoCompleteNextIncompleteCriterion();
        }

        void AutoCompleteNextIncompleteCriterion()
        {
            m_Continue = false;

            while (m_Tutorial.TryGoToNextPage())
            {
            }

            var nextIncompleteCriterion = GetNextIncompleteCriterion();
            if (nextIncompleteCriterion == null)
            {
                Stop();
                return;
            }

            if (!nextIncompleteCriterion.AutoComplete())
            {
                Debug.LogWarning("Unable to complete criterion automatically");
                Stop();
            }

            m_CompletionDeadline = EditorApplication.timeSinceStartup + k_TimeoutDuration;
        }

        Criterion GetNextIncompleteCriterion()
        {
            foreach (var page in m_Tutorial.pages)
            {
                foreach (var paragraph in page.paragraphs)
                {
                    foreach (var typedCriterion in paragraph.criteria)
                    {
                        var criterion = typedCriterion.criterion;
                        if (!criterion.completed)
                            return criterion;
                    }
                }
            }

            return null;
        }
    }
}
