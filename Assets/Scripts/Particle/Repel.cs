using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repel : MonoBehaviour
{
    public WeightsBiases.ParticleType particleType = WeightsBiases.ParticleType.Electron;
    public float strength = 6;
    public float radius = 20;
    public Floor floor;

    public enum RepelType
    {
        explosive,
        quick,
        soft
    };

    public RepelType repelType = RepelType.explosive;

    void RepelParticles(WeightsBiases.ParticleType pT)
    {
        Rigidbody rb = null;
        for (int i = 0; i < floor.particles.Count; i++) 
        {

            //if the particle is of the specified type and within a set distance
            if (floor.particles[i].GetParticleType() == pT && Vector3.Distance(floor.particles[i].transform.position, this.transform.position) < radius)
            {

                rb = floor.particles[i].GetComponent<Rigidbody>();

                Vector3 force = Vector3.zero;
                switch (repelType)
                {
                    case RepelType.quick:
                        force = -(this.transform.position - rb.position) * rb.mass * strength * 10;
                        break;

                    case RepelType.explosive:
                        if (Time.time % 5 == 0)
                        {
                            force = -(this.transform.position - rb.position) * rb.mass * strength * 100;
                        }
                        else
                        {
                            force = Vector3.zero;
                        }
                        break;

                    case RepelType.soft:
                        force = -(this.transform.position - rb.position) * rb.mass * strength * 10 *  (radius - Vector3.Distance(this.transform.position, rb.position) );
                        break;
                        
                }

                

                rb.AddForce(force);
            }
        }
    }

    void Start()
    {
    }

    void FixedUpdate()
    {

        RepelParticles(particleType);
    }

}
