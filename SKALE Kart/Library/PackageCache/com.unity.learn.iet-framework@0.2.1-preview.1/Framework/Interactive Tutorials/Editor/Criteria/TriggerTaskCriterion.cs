using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityObject = UnityEngine.Object;

namespace Unity.InteractiveTutorials
{
    public class TriggerTaskCriterion : Criterion
    {
        public enum TriggerTaskTestMode { TriggerEnter, TriggerExit, CollisionEnter, CollisionExit }

        [SerializeField]
        internal ObjectReference objectReference = new ObjectReference();
        public TriggerTaskTestMode testMode = TriggerTaskTestMode.TriggerEnter;

        bool m_Enabled;

        public override void StartTesting()
        {
            CollisionBroadcaster2D.playerEnteredCollision += OnPlayerEnteredCollision2D;
            CollisionBroadcaster2D.playerEnteredTrigger += OnPlayerEnteredTrigger2D;
            CollisionBroadcaster2D.playerExitedCollision += OnPlayerExitCollision2D;
            CollisionBroadcaster2D.playerExitedTrigger += OnPlayerExitTrigger2D;

            CollisionBroadcaster3D.playerEnteredCollision += OnPlayerEnteredCollision;
            CollisionBroadcaster3D.playerEnteredTrigger += OnPlayerEnteredTrigger;
            CollisionBroadcaster3D.playerExitedCollision += OnPlayerExitCollision;
            CollisionBroadcaster3D.playerExitedTrigger += OnPlayerExitTrigger;
        }

        public override void StopTesting()
        {
            base.StopTesting();
            CollisionBroadcaster2D.playerEnteredCollision -= OnPlayerEnteredCollision2D;
            CollisionBroadcaster2D.playerEnteredTrigger -= OnPlayerEnteredTrigger2D;
            CollisionBroadcaster2D.playerExitedCollision -= OnPlayerExitCollision2D;
            CollisionBroadcaster2D.playerExitedTrigger -= OnPlayerExitTrigger2D;

            CollisionBroadcaster3D.playerEnteredCollision -= OnPlayerEnteredCollision;
            CollisionBroadcaster3D.playerEnteredTrigger -= OnPlayerEnteredTrigger;
            CollisionBroadcaster3D.playerExitedCollision -= OnPlayerExitCollision;
            CollisionBroadcaster3D.playerExitedTrigger -= OnPlayerExitTrigger;
        }

        //Overriding the update completion state, as this criterion is not state based
        public override void UpdateCompletion()
        {
        }

        GameObject referencedGameObject
        {
            get { return objectReference.sceneObjectReference.ReferencedObjectAsGameObject;  }
        }

        void OnPlayerEnteredCollision2D(CollisionBroadcaster2D sender)
        {
            if (testMode == TriggerTaskTestMode.CollisionEnter && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerEnteredTrigger2D(CollisionBroadcaster2D sender)
        {
            if (testMode == TriggerTaskTestMode.TriggerEnter && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerExitCollision2D(CollisionBroadcaster2D sender)
        {
            if (testMode == TriggerTaskTestMode.CollisionExit && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerExitTrigger2D(CollisionBroadcaster2D sender)
        {
            if (testMode == TriggerTaskTestMode.TriggerExit && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerEnteredCollision(CollisionBroadcaster3D sender)
        {
            if (testMode == TriggerTaskTestMode.CollisionEnter && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerEnteredTrigger(CollisionBroadcaster3D sender)
        {
            if (testMode == TriggerTaskTestMode.TriggerEnter && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerExitCollision(CollisionBroadcaster3D sender)
        {
            if (testMode == TriggerTaskTestMode.CollisionExit && referencedGameObject == sender.gameObject)
                completed = true;
        }

        void OnPlayerExitTrigger(CollisionBroadcaster3D sender)
        {
            if (testMode == TriggerTaskTestMode.TriggerExit && referencedGameObject == sender.gameObject)
                completed = true;
        }

        public override bool AutoComplete()
        {
            if (referencedGameObject == null)
                return false;

            if (referencedGameObject.GetComponent<BaseCollisionBroadcaster>() == null)
                return false;

            var playerComponent = SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(gameObject => gameObject.GetComponentInChildren<IPlayerAvatar>())
                .Cast<Component>()
                .FirstOrDefault(component => component != null);

            if (playerComponent == null)
                return false;

            var playerGameObject = HandleUtilityProxy.FindSelectionBase(playerComponent.gameObject);
            if (playerGameObject == null)
                playerGameObject = playerComponent.gameObject;

            switch (testMode)
            {
                case TriggerTaskTestMode.TriggerEnter:
                case TriggerTaskTestMode.CollisionEnter:
                    playerGameObject.transform.position = referencedGameObject.transform.position;
                    return true;

                case TriggerTaskTestMode.TriggerExit:
                case TriggerTaskTestMode.CollisionExit:
                default:
                    return false;
            }
        }
    }
}
