using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace Unity.InteractiveTutorials.Tests
{
    public class AssetToSceneObjectReferenceManagerTests
    {
        private SceneObjectGUIDManager manager;

        [SetUp]
        public void InitManager()
        {
            manager = SceneObjectGUIDManager.instance = new SceneObjectGUIDManager();
        }

        [Test]
        public void Manager_WithNoRegisteredComponents_WontReturnAnyComponents()
        {
            Assert.IsFalse(manager.Contains("non_existing_id"));
        }

        [Test]
        public void Manager_WithOneRegisteredComponent_ReturnTheComponent()
        {
            var c = CreateGameObjectWithReferenceComponent();

            Assert.IsTrue(manager.Contains(c.id));
            Assert.IsNotNull(manager.GetComponent(c.id));
        }

        [Test]
        public void Manager_WithManyComponentsRegistered_WillReturnTheCorrectOnes()
        {
            var c1 = CreateGameObjectWithReferenceComponent();
            var c2 = CreateGameObjectWithReferenceComponent();
            var c3 = CreateGameObjectWithReferenceComponent();

            Assert.IsTrue(manager.Contains(c2.id));
            Assert.AreEqual(c2, manager.GetComponent(c2.id));
            Assert.AreNotEqual(c1, manager.GetComponent(c2.id));
            Assert.AreNotEqual(c3, manager.GetComponent(c2.id));
        }

        [Test]
        public void Manager_WithManyComponentsRegistered_WillReturnNullForNotExisting()
        {
            CreateGameObjectWithReferenceComponent();
            CreateGameObjectWithReferenceComponent();
            CreateGameObjectWithReferenceComponent();

            Assert.IsNull(manager.GetComponent("Not_Existing_id"));
        }

        [Test]
        public void Manager_WithComponentAddedAndRemoved_WillReturnNull()
        {
            var c = CreateGameObjectWithReferenceComponent();
            var id = c.id;
            Object.DestroyImmediate(c);

            Assert.IsNull(manager.GetComponent(id));
        }

        [Test]
        public void Manager_WithManyComponentsAddedAndOneRemoved_WillOnlyReturnTheExisting()
        {
            var c1 = CreateGameObjectWithReferenceComponent();
            var c2 = CreateGameObjectWithReferenceComponent();
            var c2Id = c2.id;
            Object.DestroyImmediate(c2);
            var c3 = CreateGameObjectWithReferenceComponent();

            Assert.IsNotNull(manager.GetComponent(c1.id));
            Assert.IsNotNull(manager.GetComponent(c3.id));
            Assert.IsNull(manager.GetComponent(c2Id));
        }

        private static SceneObjectGUIDComponent CreateGameObjectWithReferenceComponent()
        {
            var go = new GameObject();
            Undo.RegisterCreatedObjectUndo(go, "Created test GO");
            return go.AddComponent<SceneObjectGUIDComponent>();
        }
    }
}
