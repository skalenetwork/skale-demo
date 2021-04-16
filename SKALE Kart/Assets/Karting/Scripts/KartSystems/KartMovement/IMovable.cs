using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using KartGame.Track;
using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// Should be implemented by things that are controlled by user input that can be moved by something other than the user.
    /// </summary>
    public interface IMovable : IControllable
    {
        /// <summary>
        /// Should be used to move the implementing object by an amount.
        /// </summary>
        /// <param name="positionDelta">The change in position.</param>
        /// <param name="rotationDelta">The change in rotation.</param>
        void ForceMove (Vector3 positionDelta, Quaternion rotationDelta);

        /// <summary>
        /// Finds the racer timing information for this movable.
        /// </summary>
        /// <returns>Returns null if no IRacer implementation is relevant.</returns>
        IRacer GetRacer ();

        /// <summary>
        /// Finds the kart info for this movable.
        /// </summary>
        /// <returns>Returns null if no IKartInfo implementation is relevant.</returns>
        IKartInfo GetKartInfo ();
    }
}