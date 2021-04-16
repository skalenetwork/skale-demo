using System;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// This is used to mark out key points on the track that a racer must pass through in order to count as having completed a lap.
    /// </summary>
    [RequireComponent (typeof(BoxCollider))]
    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// This is subscribed to by the TrackManager in order to measure a racer's progress around the track.
        /// </summary>
        public event Action<IRacer, Checkpoint> OnKartHitCheckpoint;

        [Tooltip ("Whether or not this checkpoint is the start/finish line.")]
        public bool isStartFinishLine;
        [Tooltip ("The layers to check for a kart passing through this trigger.")]
        public LayerMask kartLayers;
        [Tooltip("The layers to check for the ground.  Used to determine where the reset position for a kart is.")]
        public LayerMask groundLayers;

        Vector3 m_ResetPosition;
        Quaternion m_ResetRotation;

        public Vector3 ResetPosition => m_ResetPosition;
        public Quaternion ResetRotation => m_ResetRotation;

        void Reset ()
        {
            GetComponent<BoxCollider> ().isTrigger = true;
            kartLayers = LayerMask.GetMask ("Default");
        }

        void Start ()
        {
            float boxColliderHeight = GetComponent<BoxCollider> ().size.y;
            Ray ray = new Ray(transform.position + Vector3.up * boxColliderHeight * 0.5f, -Vector3.up);
            
            RaycastHit[] hits = Physics.RaycastAll (ray, boxColliderHeight, groundLayers, QueryTriggerInteraction.Ignore);

            if (hits.Length == 0)
                throw new UnityException ("This checkpoint is not above ground and has no set reset position.");
            
            RaycastHit closestGround = new RaycastHit ();
            closestGround.distance = float.PositiveInfinity;
            
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < closestGround.distance)
                    closestGround = hits[i];
            }

            m_ResetPosition = closestGround.point;

            m_ResetRotation = Quaternion.LookRotation (transform.forward, closestGround.normal);
        }

        void OnTriggerEnter (Collider other)
        {
            if (kartLayers.ContainsGameObjectsLayer (other.gameObject))
            {
                IRacer racer = other.GetComponent<IRacer> ();
                if (racer != null)
                    OnKartHitCheckpoint?.Invoke (racer, this);
            }
        }
    }
}