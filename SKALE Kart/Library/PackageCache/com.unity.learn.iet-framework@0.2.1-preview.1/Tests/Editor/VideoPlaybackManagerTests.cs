using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Video;

namespace Unity.InteractiveTutorials.Tests
{
    // These tests seem problematic: they pass on macOS & 2018.3 but not on other configurations.
    public class VideoPlaybackManagerTests : TestBase
    {
        VideoPlaybackManager m_VideoPlaybackManager;
        VideoClip m_VideoClip1;
        VideoClip m_VideoClip2;

        [SetUp]
        public void SetUp()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));

            m_VideoPlaybackManager = new VideoPlaybackManager();
            m_VideoClip1 = AssetDatabase.LoadAssetAtPath<VideoClip>(GetTestAssetPath("TestVideoClip1.mov"));
            m_VideoClip2 = AssetDatabase.LoadAssetAtPath<VideoClip>(GetTestAssetPath("TestVideoClip2.mov"));
        }

        [TearDown]
        public void TearDown()
        {
            m_VideoPlaybackManager.OnDisable();
        }

        // TODO [Test]
        public void GetTextureForVideoClip_BeforeOnEnableIsCalled_ThrowsNullReferenceException()
        {
            Assert.That(() => m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1), Throws.Exception);
        }

        // TODO [Test]
        public void GetTextureForVideoClip_AfterOnEnableIsCalled_ReturnsValidTexture()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);

            Assert.That(texture != null, Is.True);
        }

        // TODO [Test]
        public void GetTextureForVideoClip_AfterOnDisableIsCalled_ThrowsNullReferenceException()
        {
            m_VideoPlaybackManager.OnEnable();
            m_VideoPlaybackManager.OnDisable();

            Assert.That(() => m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1), Throws.Exception);
        }

        // TODO [Test]
        public void GetTextureForVideoClip_ReturnsSameTextureForSameVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture1 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            var texture2 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);

            Assert.That(texture1 == texture2, Is.True);
        }

        // TODO [Test]
        public void GetTextureForVideoClip_ReturnsDifferentTextureForDifferentVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture1 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            var texture2 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);

            Assert.That(texture1 == texture2, Is.False);
        }

        // TODO [Test]
        public void GetTextureForVideoClip_OnDisable_DestroysAllObjectsCreatedByManager()
        {
            var objectsBefore = Resources.FindObjectsOfTypeAll<Object>();
            m_VideoPlaybackManager.OnEnable();
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);
            var newObjects = Resources.FindObjectsOfTypeAll<Object>().Except(objectsBefore);

            m_VideoPlaybackManager.OnDisable();

            foreach (var newObject in newObjects)
            {
                Assert.That(newObject == null, Is.True, "Object not destroyed: " + newObject);
            }
        }

        // TODO [Test]
        public void GetTextureForVideoClip_ClearCache_DestroysAllObjectsCreatedByCallsToGetTextureForVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();
            var objectsBefore = Resources.FindObjectsOfTypeAll<Object>();
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);
            var newObjects = Resources.FindObjectsOfTypeAll<Object>().Except(objectsBefore);

            m_VideoPlaybackManager.ClearCache();

            foreach (var newObject in newObjects)
            {
                Assert.That(newObject == null, Is.True, "Object not destroyed: " + newObject);
            }
        }
    }
}
