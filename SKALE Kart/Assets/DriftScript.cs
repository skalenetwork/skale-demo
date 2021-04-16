using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftScript : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up") && (Input.GetKey("left") || Input.GetKey("right")))
        {
            var em = ps.emission;
            em.enabled = true;
        }
        else
        {
            var em = ps.emission;
            em.enabled = false;
        }

    }
}
