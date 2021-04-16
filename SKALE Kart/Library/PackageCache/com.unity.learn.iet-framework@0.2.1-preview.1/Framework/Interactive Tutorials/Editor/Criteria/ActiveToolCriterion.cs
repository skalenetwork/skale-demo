using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public class ActiveToolCriterion : Criterion
    {
        public Tool targetTool { get { return m_TargetTool; } set { m_TargetTool = value; } }
        [SerializeField]
        Tool m_TargetTool;

        public override void StartTesting()
        {
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        public override void StopTesting()
        {
            EditorApplication.update -= UpdateCompletion;
        }

        protected override bool EvaluateCompletion()
        {
            return Tools.current == m_TargetTool;
        }

        public override bool AutoComplete()
        {
            Tools.current = m_TargetTool;
            return true;
        }
    }
}
