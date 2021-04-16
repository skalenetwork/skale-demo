using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public static class GUIViewDebuggerHelperProxy
    {
        public static void GetViews(List<GUIViewProxy> views)
        {
            var guiViews = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(guiViews);
            views.AddRange(guiViews.Select(view => new GUIViewProxy(view)));
        }

        public static void DebugWindow(GUIViewProxy guiViewProxy)
        {
            GUIViewDebuggerHelper.DebugWindow(guiViewProxy.guiView);
        }

        public static void GetDrawInstructions(List<IMGUIDrawInstructionProxy> drawInstructions)
        {
            var instructions = new List<IMGUIDrawInstruction>();
            GUIViewDebuggerHelper.GetDrawInstructions(instructions);
            drawInstructions.AddRange(instructions.Select(i => new IMGUIDrawInstructionProxy(i)));
        }

        public static void GetNamedControlInstructions(List<IMGUINamedControlInstructionProxy> namedControlInstructions)
        {
            var instructions = new List<IMGUINamedControlInstruction>();
            GUIViewDebuggerHelper.GetNamedControlInstructions(instructions);
            namedControlInstructions.AddRange(instructions.Select(i => new IMGUINamedControlInstructionProxy(i)));
        }

        public static void GetPropertyInstructions(List<IMGUIPropertyInstructionProxy> propertyInstructions)
        {
            var instructions = new List<IMGUIPropertyInstruction>();
            GUIViewDebuggerHelper.GetPropertyInstructions(instructions);
            propertyInstructions.AddRange(instructions.Select(i => new IMGUIPropertyInstructionProxy(i)));
        }

        public static void GetUnifiedInstructions(List<IMGUIInstructionProxy> unifiedInstructions)
        {
            var instructions = new List<IMGUIInstruction>();
            GUIViewDebuggerHelper.GetUnifiedInstructions(instructions);
            unifiedInstructions.AddRange(instructions.Select(i => new IMGUIInstructionProxy(i)));
        }

        public static void StopDebugging()
        {
            GUIViewDebuggerHelper.StopDebugging();
        }
    }

    public class IMGUIDrawInstructionProxy
    {
        IMGUIDrawInstruction m_IMGUIDrawInstruction;

        internal IMGUIDrawInstructionProxy(IMGUIDrawInstruction imguiDrawInstruction)
        {
            m_IMGUIDrawInstruction = imguiDrawInstruction;
        }

        public GUIContent usedGUIContent { get { return m_IMGUIDrawInstruction.usedGUIContent; } }
        public Rect rect { get { return m_IMGUIDrawInstruction.rect; } }
        public string usedGUIStyleName { get { return m_IMGUIDrawInstruction.usedGUIStyle.name; } }
    }

    public class IMGUINamedControlInstructionProxy
    {
        IMGUINamedControlInstruction m_IMGUINamedControlInstructionProxy;

        internal IMGUINamedControlInstructionProxy(IMGUINamedControlInstruction imguiNamedControlInstruction)
        {
            m_IMGUINamedControlInstructionProxy = imguiNamedControlInstruction;
        }

        public string name { get { return m_IMGUINamedControlInstructionProxy.name; } }
        public Rect rect { get { return m_IMGUINamedControlInstructionProxy.rect; } }
    }

    public class IMGUIPropertyInstructionProxy
    {
        IMGUIPropertyInstruction m_IMGUIPropertyInstruction;

        internal IMGUIPropertyInstructionProxy(IMGUIPropertyInstruction imguiPropertyInstruction)
        {
            m_IMGUIPropertyInstruction = imguiPropertyInstruction;
        }

        public string targetTypeName { get { return m_IMGUIPropertyInstruction.targetTypeName; } }
        public string path { get { return m_IMGUIPropertyInstruction.path; } }
        public Rect rect { get { return m_IMGUIPropertyInstruction.rect; } }
    }

    public enum InstructionTypeProxy
    {
        StyleDraw = 1,
        ClipPush = 2,
        ClipPop = 3,
        LayoutBeginGroup = 4,
        LayoutEndGroup = 5,
        LayoutEntry = 6,
        PropertyBegin = 7,
        PropertyEnd = 8,
        LayoutNamedControl = 9,
    }

    public class IMGUIInstructionProxy
    {
        IMGUIInstruction m_IMGUIInstruction;

        internal IMGUIInstructionProxy(IMGUIInstruction imguiInstruction)
        {
            m_IMGUIInstruction = imguiInstruction;
        }

        public InstructionTypeProxy type { get { return (InstructionTypeProxy)m_IMGUIInstruction.type; } }
        public int level { get { return m_IMGUIInstruction.level; } }
        public int typeInstructionIndex { get { return m_IMGUIInstruction.typeInstructionIndex; } }
    }
}
