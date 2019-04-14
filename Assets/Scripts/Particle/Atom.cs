using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Atom : MonoBehaviour
{
    private float[] atomBias = new float[3] {1, 1, 1};
    public float[] atomCount = new float[3] {0,0,0};
    public float maxDistance = 30;
    public float radius = 20;
    public float forceMultiplier = 10;
    public float timeMultiplier = 1;
    public float rotationTimeMultiplier1 = 1;
    public float rotationTimeMultiplier2 = 1;
    public float particleMass = 10;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public GameObject mainFloor;
    private Floor floor;
    public List<WeightsBiases> particles = new List<WeightsBiases>();

    public void CalculateBias()
    {
        int totalNumber = (int)(atomCount[0] + atomCount[1] + (atomCount[2] / Mathf.Clamp(atomCount[0], 1, 1000)) );
        if (totalNumber > 0)
        {
            atomBias[0] = 0.875f - (atomCount[0] / totalNumber);
            atomBias[1] = 0.875f - (atomCount[1] / totalNumber);
            atomBias[2] = 0.875f - ( (atomCount[2] / Mathf.Clamp(atomCount[0],1,1000) )  / totalNumber);

            Debug.Log("Total mass: "+totalNumber+" protons bias: " + atomBias[0] + " neutron bias: " + atomBias[1] + " electron bias: " + atomBias[2]);
        }
        else
        {
            atomBias[0] = 1;
            atomBias[1] = 1;
            atomBias[2] = 1;
        }
        
    }

    public void FindParticlesNearAtom()
    {
        //loops through the particles attached to the floor and sees if any aare close enough to grab
        for (int i = 0; i < floor.particles.Count; i++)
        {
            WeightsBiases newParticle = floor.particles[i];
            float distance = Vector3.Distance(newParticle.transform.position, this.transform.position);

            //if within distance and valid it adds it to the atom
            if (newParticle != null && distance <= maxDistance && !particles.Contains(newParticle))
            {
                particles.Add(newParticle); //grabs the particle
                atomCount[(int) newParticle.GetParticleType()]++;
            }

            //if the particle has been removed or is too far off it removes it from the atom
            if (particles.Contains(newParticle) && (newParticle == null || distance > maxDistance ))
            {
                particles.Remove(newParticle); //removes the particles
                atomCount[(int)newParticle.GetParticleType()]--;
            }
        }
        CalculateBias();
    }

    public void ForceParticlesAttachedToAtom()
    {
        int electronIndex = 0;
        for (int i = 0; i < particles.Count; i++)
        {
            Vector3 force = new Vector3(0, 0, 0);

            Rigidbody particle = particles[i].GetComponent<Rigidbody>();

            if (particle != null)
            {
                float inverseDistance =
                    Mathf.Clamp((maxDistance - Vector3.Distance(particle.position, this.transform.position)) / maxDistance, 0, 1);

                if (particles[i].GetParticleType() == WeightsBiases.ParticleType.Electron) {
                    //offset for electron circlular rotation
                    //radius = (0.5f*radius) + (radius * (electronIndex / atomCount[2]) * 0.5f);
                    Vector3 offset =
                        new Vector3(radius * Mathf.Sin((timeMultiplier * Time.time) + ((electronIndex / atomCount[2]) * 2 * Mathf.PI)), 0,
                            radius * Mathf.Cos((timeMultiplier * Time.time) + ((electronIndex / atomCount[2]) * 2 * Mathf.PI)));

                    //rotates the offset
                    Quaternion rotation = Quaternion.Euler(45 * Mathf.Cos(atomCount[0] * 2 * Mathf.PI * (electronIndex / atomCount[2])), 0, 45 * Mathf.Sin(atomCount[1] * 2 * Mathf.PI * (electronIndex / atomCount[2])));
                    offset = rotation * offset;


                    force = (this.transform.position - particle.position + offset) * particle.mass * forceMultiplier * inverseDistance * atomBias[(int)particles[i].type];
                    electronIndex++;

                }
                else {
                    force = (this.transform.position - particle.position) * particle.mass * forceMultiplier * inverseDistance * atomBias[(int)particles[i].type];
                }

                particle.AddForce(force);
            }
        }
    }

    public void DestroyAtom()
    {
        Debug.Log("boom");
        //maxDistance = 0;
        for (int i = 0; i < particles.Count; i++)
        {
            Rigidbody rb = particles[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(1000000,this.transform.position,40);
        }

        atomCount[0] = 0;
        atomCount[1] = 0;
        atomCount[2] = 0;
        particles.Clear();
        maxDistance = 0;
    }

    public void ResetAtom()
    {
        //restores lists but doesnt remove atom
    }

    void Start()
    {
        floor = mainFloor.GetComponent<Floor>();
        particles.Clear();
    }

    void Update()
    {
        
        FindParticlesNearAtom();

        if ((int) Time.time % 60 == 0 && (int)Time.time !=0)
        {
            
            //DestroyAtom();
        }
    }

    void FixedUpdate()
    {
        ForceParticlesAttachedToAtom();
    }

}
