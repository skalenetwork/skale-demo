using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// This can work as a placeholder for kart modifiers that have yet to be created.  It does not modify the stats at all.
    /// </summary>
    public struct DefaultKartModifier : IKartModifier
    {
        public float ModifyAcceleration (float acceleration)
        {
            return acceleration;
        }

        public float ModifyBraking (float braking)
        {
            return braking;
        }

        public float ModifyCoastingDrag (float coastingDrag)
        {
            return coastingDrag;
        }

        public float ModifyGravity (float gravity)
        {
            return gravity;
        }

        public float ModifyGrip (float grip)
        {
            return grip;
        }

        public float ModifyHopHeight (float hopHeight)
        {
            return hopHeight;
        }

        public float ModifyReverseAcceleration (float reverseAcceleration)
        {
            return reverseAcceleration;
        }

        public float ModifyReverseSpeed (float reverseSpeed)
        {
            return reverseSpeed;
        }

        public float ModifyTopSpeed (float topSpeed)
        {
            return topSpeed;
        }

        public float ModifyTurnSpeed (float turnSpeed)
        {
            return turnSpeed;
        }

        public float ModifyWeight (float weight)
        {
            return weight;
        }
    }
}