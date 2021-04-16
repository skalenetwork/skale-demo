using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// Should be implemented by anything wishing to modify the stats of a kart.  This can be anything from a patch of mud on the ground to pickups to the driver of the kart.
    /// Each function should return the adjusted relevant kart stat.
    /// </summary>
    public interface IKartModifier
    {
        /// <summary>
        /// Used to change the current acceleration from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="acceleration">The current acceleration.</param>
        /// <returns>The changed acceleration.</returns>
        float ModifyAcceleration (float acceleration);

        /// <summary>
        /// Used to change the current braking from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="braking">The current braking.</param>
        /// <returns>The changed braking.</returns>
        float ModifyBraking (float braking);

        /// <summary>
        /// Used to change the current coastingDrag from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="coastingDrag">The current coastingDrag.</param>
        /// <returns>The changed coastingDrag.</returns>
        float ModifyCoastingDrag (float coastingDrag);

        /// <summary>
        /// Used to change the current gravity from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="gravity">The current gravity.</param>
        /// <returns>The changed gravity.</returns>
        float ModifyGravity (float gravity);

        /// <summary>
        /// Used to change the current grip from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="grip">The current grip.</param>
        /// <returns>The changed grip.</returns>
        float ModifyGrip (float grip);

        /// <summary>
        /// Used to change the current acceleration from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="acceleration">The current acceleration.</param>
        /// <returns>The changed acceleration.</returns>
        float ModifyHopHeight (float hopHeight);

        /// <summary>
        /// Used to change the current reverseAcceleration from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="reverseAcceleration">The current reverseAcceleration.</param>
        /// <returns>The changed reverseAcceleration.</returns>
        float ModifyReverseAcceleration (float reverseAcceleration);

        /// <summary>
        /// Used to change the current reverseSpeed from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="reverseSpeed">The current reverseSpeed.</param>
        /// <returns>The changed reverseSpeed.</returns>
        float ModifyReverseSpeed (float reverseSpeed);

        /// <summary>
        /// Used to change the current topSpeed from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="topSpeed">The current topSpeed.</param>
        /// <returns>The changed topSpeed.</returns>
        float ModifyTopSpeed (float topSpeed);

        /// <summary>
        /// Used to change the current turnSpeed from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="turnSpeed">The current turnSpeed.</param>
        /// <returns>The changed turnSpeed.</returns>
        float ModifyTurnSpeed (float turnSpeed);

        /// <summary>
        /// Used to change the current weight from one value to another, usually by multiplying or adding to it.
        /// </summary>
        /// <param name="weight">The current weight.</param>
        /// <returns>The changed weight.</returns>
        float ModifyWeight (float weight);
    }
}