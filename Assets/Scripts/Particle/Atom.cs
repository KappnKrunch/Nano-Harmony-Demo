using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Atom : MonoBehaviour
{
    private float[] atomBias = new float[3] {1, 1, 1};
    public float[] atomCount = new float[3] {0,0,0};
    public float maxDistance = 30;
    public float forceMultiplier = 10;
    public float timeMultiplier = 1;
    public float rotationTimeMultiplier = 1;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public GameObject mainFloor;
    private Floor floor;
    public List<WeightsBiases> particles = new List<WeightsBiases>();

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
    }

    public void ForceParticlesAttachedToAtom()
    {
        int electronIndex = 0;
        for (int i = 0; i < particles.Count; i++)
        {
            Vector3 force = new Vector3(0, 0, 0);

            Rigidbody particle = particles[i].GetComponent<Rigidbody>();

            float inverseDistance =
                Mathf.Clamp((maxDistance - Vector3.Distance(particle.position, this.transform.position)) / maxDistance, 0, 1);

            if (particles[i].GetParticleType() == WeightsBiases.ParticleType.Electron)
            {
                //offset for electron circlular rotation
                Vector3 offset =
                    new Vector3(15 * Mathf.Sin((timeMultiplier * Time.time) + ((electronIndex / atomCount[2]) * 2 * Mathf.PI)), 0,
                        15 * Mathf.Cos((timeMultiplier * Time.time) + ((electronIndex / atomCount[2]) * 2 * Mathf.PI)));

                //rotates the offset
                Quaternion rotation = Quaternion.Euler(180*Mathf.Cos(rotationTimeMultiplier * 3 * Mathf.PI * (electronIndex / atomCount[2])),0,0);
                offset = rotation * offset;

                force = (this.transform.position - particle.position + offset) * particle.mass * forceMultiplier * inverseDistance;
                electronIndex++;
                
            }
            else
            {
                force = (this.transform.position - particle.position) * particle.mass * forceMultiplier * inverseDistance;
            }

            particle.AddForce(force);
        }
    }


    public void DestroyAtom()
    {
        //deletes the atom empty and seperates each particle
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
    }

    void FixedUpdate()
    {
        ForceParticlesAttachedToAtom();
    }

}
