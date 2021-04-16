using System.Collections;
using NUnit.Framework;
using Unity.InteractiveTutorials;
using Unity.InteractiveTutorials.Tests;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Windows;

namespace Unity.InteractiveTutorials.Tests
{
    public class BuildStartedCriterionTests : CriterionTestBase<BuildStartedCriterion>
    {
        [UnityTest]
        public IEnumerator CustomHandlerIsInvoked_IsCompleted()
        {
#if UNITY_EDITOR_WIN
            var target = BuildTarget.StandaloneWindows;
            var locationPathName = "Test/Test.exe";
#elif UNITY_EDITOR_OSX
            var target = BuildTarget.StandaloneOSX;
            var locationPathName = "Test/Test";
#else
#error Unsupported platform
#endif

            m_Criterion.BuildPlayerCustomHandler(new BuildPlayerOptions
            {
                scenes = null,
                target = target,
                locationPathName = locationPathName,
                targetGroup = BuildTargetGroup.Unknown
            });
            yield return null;

            Assert.IsTrue(m_Criterion.completed);

            // Cleanup
            if (Directory.Exists("Test"))
            {
                Directory.Delete("Test");
            }
        }

        [UnityTest]
        public IEnumerator AutoComplete_IsCompleted()
        {
            yield return null;
            Assert.IsTrue(m_Criterion.AutoComplete());
        }
    }
}
