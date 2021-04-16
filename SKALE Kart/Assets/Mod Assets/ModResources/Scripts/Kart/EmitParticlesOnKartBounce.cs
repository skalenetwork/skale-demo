using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticlesOnKartBounce : MonoBehaviour
{

#if UNITY_TEMPLATE_KART

    ParticleSystem p;

    private void Awake() {
        p = GetComponent<ParticleSystem>();
        var kart = GetComponentInParent<KartGame.KartSystems.KartMovement>();

        //TODO:
        //should remove this once the template is fixed.
        //need to clamp capsule size as kart's collider is too big for Bounce to work in the default template.
        var capsule = kart.GetComponent<CapsuleCollider>();
        capsule.height = Mathf.Clamp(capsule.height, 0, 1f);

        kart.OnKartCollision.AddListener(KartCollision_OnExecute);
    }

    void KartCollision_OnExecute() {
        p.Play();
    }


#endif

}
