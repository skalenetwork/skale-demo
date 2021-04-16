using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A ScriptableObject that takes the current stats and multiplies each.
    /// </summary>
    [CreateAssetMenu] public class MultiplicativeKartModifierAsset : ScriptableObject, IKartModifier
    {
        [Tooltip ("The stats to be multiplied by the current stats.")]
        public KartStats statMultipliers = new KartStats (1f);

        public float ModifyAcceleration (float acceleration)
        {
            return acceleration * statMultipliers.acceleration;
        }

        public float ModifyBraking (float braking)
        {
            return braking * statMultipliers.braking;
        }

        public float ModifyCoastingDrag (float coastingDrag)
        {
            return coastingDrag * statMultipliers.coastingDrag;
        }

        public float ModifyGravity (float gravity)
        {
            return gravity * statMultipliers.gravity;
        }

        public float ModifyGrip (float grip)
        {
            return grip * statMultipliers.grip;
        }

        public float ModifyHopHeight (float hopHeight)
        {
            return hopHeight * statMultipliers.hopHeight;
        }

        public float ModifyReverseAcceleration (float reverseAcceleration)
        {
            return reverseAcceleration * statMultipliers.reverseAcceleration;
        }

        public float ModifyReverseSpeed (float reverseSpeed)
        {
            return reverseSpeed * statMultipliers.reverseSpeed;
        }

        public float ModifyTopSpeed (float topSpeed)
        {
            return topSpeed * statMultipliers.topSpeed;
        }

        public float ModifyTurnSpeed (float turnSpeed)
        {
            return turnSpeed * statMultipliers.turnSpeed;
        }

        public float ModifyWeight (float weight)
        {
            return weight * statMultipliers.weight;
        }
    }
}