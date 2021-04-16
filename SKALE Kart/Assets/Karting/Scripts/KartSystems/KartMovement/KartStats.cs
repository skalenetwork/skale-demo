using System;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A class representing the fundamental stats required to move the kart.  They are separated in this way in order to easily adjust any part of how the kart behaves.
    /// </summary>
    [Serializable] public struct KartStats
    {
        [Tooltip ("The rate at which the kart increases its forward speed when the accelerate button is held and the brake button isn't held.")]
        public float acceleration;
        [Tooltip ("The rate at which the kart decreases its forward speed when the brake button is held.")]
        public float braking;
        [Tooltip ("The rate at which the kart decreases its forward speed when neither the accelerate nor the brake button is held.")]
        public float coastingDrag;
        [Tooltip ("The rate at which the kart decreases its vertical speed when not on the ground.")]
        public float gravity;
        [Tooltip ("The rate at which the kart changes its lateral speed towards zero.")]
        public float grip;
        [Tooltip ("The speed given to the kart when a hop is performed.")]
        public float hopHeight;
        [Tooltip ("The rate at which the kart increases its backward speed when the brake button is held.")]
        public float reverseAcceleration;
        [Tooltip ("The maximum speed the kart can travel backwards.")]
        public float reverseSpeed;
        [Tooltip ("The maximum speed the kart can travel forwards.")]
        public float topSpeed;
        [Tooltip ("The rate at which the kart changes its rotation around its local y axis.")]
        public float turnSpeed;
        [Tooltip ("Used for determining which kart bounces off the other when two karts collide.")]
        public float weight;

        /// <summary>
        /// Used to assign a default value to all of the stats.
        /// </summary>
        /// <param name="defaultValue">The value to be given to each stat.</param>
        public KartStats (float defaultValue)
        {
            acceleration = defaultValue;
            braking = defaultValue;
            coastingDrag = defaultValue;
            gravity = defaultValue;
            grip = defaultValue;
            hopHeight = defaultValue;
            reverseAcceleration = defaultValue;
            reverseSpeed = defaultValue;
            topSpeed = defaultValue;
            turnSpeed = defaultValue;
            weight = defaultValue;
        }

        /// <summary>
        /// A method for applying a collection of modifications to some stats.
        /// </summary>
        /// <param name="modifiers">A collection of modifiers to the kart stats to be applied in order.</param>
        /// <param name="startingStats">The initial stats to be modified.</param>
        /// <param name="modifiedStats">The result of modifying the starting stats.</param>
        public static void GetModifiedStats (List<IKartModifier> modifiers, KartStats startingStats, ref KartStats modifiedStats)
        {
            modifiedStats.acceleration = startingStats.acceleration;
            modifiedStats.braking = startingStats.braking;
            modifiedStats.coastingDrag = startingStats.coastingDrag;
            modifiedStats.gravity = startingStats.gravity;
            modifiedStats.grip = startingStats.grip;
            modifiedStats.hopHeight = startingStats.hopHeight;
            modifiedStats.reverseAcceleration = startingStats.reverseAcceleration;
            modifiedStats.reverseSpeed = startingStats.reverseSpeed;
            modifiedStats.topSpeed = startingStats.topSpeed;
            modifiedStats.turnSpeed = startingStats.turnSpeed;
            modifiedStats.weight = startingStats.weight;

            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiedStats.acceleration = modifiers[i].ModifyAcceleration (modifiedStats.acceleration);
                modifiedStats.braking = modifiers[i].ModifyBraking (modifiedStats.braking);
                modifiedStats.coastingDrag = modifiers[i].ModifyCoastingDrag (modifiedStats.coastingDrag);
                modifiedStats.gravity = modifiers[i].ModifyGravity (modifiedStats.gravity);
                modifiedStats.grip = modifiers[i].ModifyGrip (modifiedStats.grip);
                modifiedStats.hopHeight = modifiers[i].ModifyHopHeight (modifiedStats.hopHeight);
                modifiedStats.reverseAcceleration = modifiers[i].ModifyReverseAcceleration (modifiedStats.reverseAcceleration);
                modifiedStats.reverseSpeed = modifiers[i].ModifyReverseSpeed (modifiedStats.reverseSpeed);
                modifiedStats.topSpeed = modifiers[i].ModifyTopSpeed (modifiedStats.topSpeed);
                modifiedStats.turnSpeed = modifiers[i].ModifyTurnSpeed (modifiedStats.turnSpeed);
                modifiedStats.weight = modifiers[i].ModifyWeight (modifiedStats.weight);
            }
        }
    }
}