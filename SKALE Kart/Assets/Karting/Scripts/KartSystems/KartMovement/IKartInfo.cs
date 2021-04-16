using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// An interface representing all the information about the movement of a kart that is used to make decisions about things such as animation.
    /// </summary>
    public interface IKartInfo
    {
        /// <summary>
        /// The current position of the kart.
        /// </summary>
        Vector3 Position { get; }
        /// <summary>
        /// The current rotation of the kart.
        /// </summary>
        Quaternion Rotation { get; }
        /// <summary>
        /// The current stats of the kart.
        /// </summary>
        KartStats CurrentStats { get; }
        /// <summary>
        /// The current velocity of the kart in meters per second.
        /// </summary>
        Vector3 Velocity { get; }
        /// <summary>
        /// The actual amount moved by the kart in the last FixedUpdate.
        /// </summary>
        Vector3 Movement { get; }
        /// <summary>
        /// The current speed the kart is moving forwards in meters per second.
        /// </summary>
        float LocalSpeed { get; }
        /// <summary>
        /// Whether or not the kart is currently on the ground.
        /// </summary>
        bool IsGrounded { get; }
        /// <summary>
        /// More detailed information about the ground the kart is currently on.
        /// </summary>
        GroundInfo CurrentGroundInfo { get; }
    }
}