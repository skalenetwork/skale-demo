using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// An interface representing the input controls a kart needs.
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Used for determining whether the kart should increase its forward speed.
        /// </summary>
        float Acceleration { get; }

        /// <summary>
        /// Used for turning the kart left and right.
        /// </summary>
        float Steering { get; }

        /// <summary>
        /// Not implemented by this template.  Potentially used for activating a boost.
        /// </summary>
        bool BoostPressed { get; }

        /// <summary>
        /// Not implemented by this template.  Potentially used for activating weapons/pickups.
        /// </summary>
        bool FirePressed { get; }

        /// <summary>
        /// Used for determining when the kart should hop.  Also used to initiate a drift.
        /// </summary>
        bool HopPressed { get; }

        /// <summary>
        /// Used to determine when a drift should continue.
        /// </summary>
        bool HopHeld { get; }
    }
}