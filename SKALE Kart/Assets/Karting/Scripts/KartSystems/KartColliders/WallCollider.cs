using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// This class should be placed on all wall gameobjects such that the kart can react correctly to them.
    /// </summary>
    [RequireComponent (typeof(Collider))] public class WallCollider : MonoBehaviour, IKartCollider
    {
        [Tooltip ("The amount by which the kart bounces off the wall.  A value of 0.1 means 1.1 times the velocity into the wall is the velocity away from the wall.  A minimum value of 0.1 is suggested.")]
        public float bounciness = 0.1f;

        public Vector3 ModifyVelocity (IKartInfo collidingKart, RaycastHit collisionHit)
        {
            //if the normal of collision points almost straight up or down, don't bounce
            if (Mathf.Abs(Vector3.Dot(collisionHit.normal, Vector3.up)) > .2f) return collidingKart.Velocity;

            Vector3 modifiedVelocity = collidingKart.Velocity;

            if (collidingKart.IsGrounded)
                modifiedVelocity = Vector3.ProjectOnPlane (modifiedVelocity, collidingKart.CurrentGroundInfo.normal);

            modifiedVelocity -= Vector3.Project (modifiedVelocity, collisionHit.normal) * (1f + bounciness);

            return modifiedVelocity;
        }
    }
}