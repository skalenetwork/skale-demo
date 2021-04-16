using System;
using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// A abstract class that deals with the repositioning of karts when they get stuck, fall off the track, etc.
    /// Derive from this to implement a way of repositioning karts.
    /// </summary>
    public abstract class KartRepositioner : MonoBehaviour
    {
        /// <summary>
        /// This event is called when the Reposition function is finished.
        /// </summary>
        public event Action<IMovable, bool> OnRepositionComplete;

        /// <summary>
        /// Should be called to reposition a kart to the last checkpoint it passed through.
        /// This must be overriden by a child class and can perform any operation over any period of time but must call RepositionComplete immediately before returning.
        /// </summary>
        /// <param name="lastCheckpoint">The last checkpoint the kart passed through.</param>
        /// <param name="movable">The IMovable implementation representing the kart.</param>
        /// <param name="isControlled">Whether or not the kart is being controlled.  This informs whether control should be re-enabled after the reposition.</param>
        public abstract void Reposition (Checkpoint lastCheckpoint, IMovable movable, bool isControlled);

        /// <summary>
        /// This function called immediately before the overriden Reposition function returns.
        /// </summary>
        protected void RepositionComplete (IMovable movable, bool doEnableControl)
        {
            OnRepositionComplete?.Invoke (movable, doEnableControl);
        }
    }
}