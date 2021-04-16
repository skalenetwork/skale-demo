using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// Should be implemented by MonoBehaviours which are on objects with which a kart can collide.
    /// </summary>
    public interface IKartCollider
    {
        /// <summary>
        /// Should be used to change the velocity of a kart when it collides with this object.
        /// </summary>
        /// <param name="collidingKart">The kart colliding with this object.</param>
        /// <param name="collisionHit">Data about the collision.</param>
        /// <returns>The velocity of the kart after it has been modified by this collision.</returns>
        Vector3 ModifyVelocity (IKartInfo collidingKart, RaycastHit collisionHit);
    }
}