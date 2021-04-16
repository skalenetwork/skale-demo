using System;
using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A plain serializable class that takes the current stats and adds to each.
    /// </summary>
    [Serializable] public class AddativeKartModifier : IKartModifier
    {
        [Tooltip ("The stats to be added to the current stats.")]
        public KartStats modifiers;

        /// <summary>
        /// Initialises the KartStats to a default value of 0.
        /// </summary>
        public AddativeKartModifier ()
        {
            modifiers = new KartStats (0f);
        }

        public float ModifyAcceleration (float acceleration)
        {
            return modifiers.acceleration + acceleration;
        }

        public float ModifyBraking (float braking)
        {
            return modifiers.braking + braking;
        }

        public float ModifyCoastingDrag (float coastingDrag)
        {
            return modifiers.coastingDrag + coastingDrag;
        }

        public float ModifyGravity (float gravity)
        {
            return modifiers.gravity + gravity;
        }

        public float ModifyGrip (float grip)
        {
            return modifiers.grip + grip;
        }

        public float ModifyHopHeight (float hopHeight)
        {
            return modifiers.hopHeight + hopHeight;
        }

        public float ModifyReverseAcceleration (float reverseAcceleration)
        {
            return modifiers.reverseAcceleration + reverseAcceleration;
        }

        public float ModifyReverseSpeed (float reverseSpeed)
        {
            return modifiers.reverseSpeed + reverseSpeed;
        }

        public float ModifyTopSpeed (float topSpeed)
        {
            return modifiers.topSpeed + topSpeed;
        }

        public float ModifyTurnSpeed (float turnSpeed)
        {
            return modifiers.turnSpeed + turnSpeed;
        }

        public float ModifyWeight (float weight)
        {
            return modifiers.weight + weight;
        }
    }
}