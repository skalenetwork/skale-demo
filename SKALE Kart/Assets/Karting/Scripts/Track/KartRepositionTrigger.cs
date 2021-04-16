using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// This class exists only as a way of triggering a kart reset in the template and should not be used as part of a full game.
    /// A replacement for a full game would detect when the kart was stuck/out of bounds/any other requirement and then replace it.
    /// </summary>
    public class KartRepositionTrigger : MonoBehaviour
    {
        [Tooltip("A reference to the KartMovement script of the kart.")]
        [RequireInterface (typeof(IMovable))]
        public Object movable;
        [Tooltip("A reference to the TrackManager script for this track.")]
        public TrackManager trackManager;

        IMovable m_Movable;

        void Awake ()
        {
            m_Movable = (IMovable)movable;
        }

        void Update ()
        {
            if (Input.GetButtonDown ("Reset"))
            {
                trackManager.ReplaceMovable (m_Movable);
            }
        }
    }
}