using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [RequireComponent(typeof(Collider))]
    public class CollisionBroadcaster3D : BaseCollisionBroadcaster
    {
        public delegate void EventHandler(CollisionBroadcaster3D sender);

        public static event EventHandler playerEnteredCollision = null;
        public static event EventHandler playerExitedCollision = null;
        public static event EventHandler playerEnteredTrigger = null;
        public static event EventHandler playerExitedTrigger = null;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerEnteredCollision != null)
                    playerEnteredCollision(this);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerExitedCollision != null)
                    playerExitedCollision(this);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.GetComponent<IPlayerAvatar>() != null)
            {
                if (playerEnteredTrigger != null)
                {
                    playerEnteredTrigger(this);
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerExitedTrigger != null)
                    playerExitedTrigger(this);
            }
        }
    }
}
