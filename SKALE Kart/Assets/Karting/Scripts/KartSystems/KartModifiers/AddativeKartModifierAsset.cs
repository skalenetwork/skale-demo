using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A ScriptableObject that takes the current stats and adds to each.
    /// </summary>
    [CreateAssetMenu] public class AddativeKartModifierAsset : ScriptableObject, IKartModifier
    {
        [Tooltip ("The stats to be added to the current stats.")]
        public KartStats statAdditions = new KartStats (0f);

        public float ModifyAcceleration (float acceleration)
        {
            return acceleration + statAdditions.acceleration;
        }

        public float ModifyBraking (float braking)
        {
            return braking + statAdditions.braking;
        }

        public float ModifyCoastingDrag (float coastingDrag)
        {
            return coastingDrag + statAdditions.coastingDrag;
        }

        public float ModifyGravity (float gravity)
        {
            return gravity + statAdditions.gravity;
        }

        public float ModifyGrip (float grip)
        {
            return grip + statAdditions.grip;
        }

        public float ModifyHopHeight (float hopHeight)
        {
            return hopHeight + statAdditions.hopHeight;
        }

        public float ModifyReverseAcceleration (float reverseAcceleration)
        {
            return reverseAcceleration + statAdditions.reverseAcceleration;
        }

        public float ModifyReverseSpeed (float reverseSpeed)
        {
            return reverseSpeed + statAdditions.reverseSpeed;
        }

        public float ModifyTopSpeed (float topSpeed)
        {
            return topSpeed + statAdditions.topSpeed;
        }

        public float ModifyTurnSpeed (float turnSpeed)
        {
            return turnSpeed + statAdditions.turnSpeed;
        }

        public float ModifyWeight (float weight)
        {
            return weight + statAdditions.weight;
        }
    }
}