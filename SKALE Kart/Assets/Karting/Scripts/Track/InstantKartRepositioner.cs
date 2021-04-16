using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// A child class of KartRepositioner that positions a kart without any delay.
    /// </summary>
    public class InstantKartRepositioner : KartRepositioner
    {
        /// <inheritdoc />
        public override void Reposition (Checkpoint lastCheckpoint, IMovable movable, bool isControlled)
        {
            IKartInfo kartInfo = movable.GetKartInfo ();

            if (kartInfo == null)
            {
                RepositionComplete (movable, isControlled);
                return;
            }

            Vector3 kartToResetPosition = lastCheckpoint.ResetPosition - kartInfo.Position;
            Quaternion kartToResetRotation = lastCheckpoint.ResetRotation * Quaternion.Inverse (kartInfo.Rotation);
            
            movable.ForceMove (kartToResetPosition, kartToResetRotation);
            
            RepositionComplete (movable, isControlled);
        }
    }
}