using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionBroadcaster2D : BaseCollisionBroadcaster
    {
        public delegate void EventHandler(CollisionBroadcaster2D sender);

        public static event EventHandler playerEnteredCollision = null;
        public static event EventHandler playerExitedCollision = null;
        public static event EventHandler playerEnteredTrigger = null;
        public static event EventHandler playerExitedTrigger = null;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerEnteredCollision != null)
                    playerEnteredCollision(this);
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerExitedCollision != null)
                    playerExitedCollision(this);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerEnteredTrigger != null)
                    playerEnteredTrigger(this);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponentInChildren<IPlayerAvatar>() != null)
            {
                if (playerExitedTrigger != null)
                    playerExitedTrigger(this);
            }
        }
    }
}
