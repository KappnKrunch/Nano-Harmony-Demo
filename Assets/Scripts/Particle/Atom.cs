using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Atom : MonoBehaviour
{
    public float attractionDistance = 20;
    public int maxParticles = 7;
    public int secondsBetweenUpdates = 3;

    public List<Particle> particles = new List<Particle>();

    private int electronCount = 0;
    private int electronSinOffset = 0;

    //Particel stats
    private float protonAmount = 0;
    private float neutronAmount = 0;
    private float electronAmount = 0;

    private float[] bias = new float[3]; //how strong each particle is pulled towrads atom

    public void CalculateBias()
    {
        float total = neutronAmount + protonAmount + electronAmount;

        //inverse attraction
        bias[0] = 1 - (protonAmount / total);
        bias[1] = 1 - (neutronAmount / total);
        bias[2] = 1 - (electronAmount / total);
        Debug.Log(total);
    }

    public void CheckParticle()
    {
        //finds all particles and sees if they arent already attached to an atom, if they arent it adds it to the attached list

        //check all particles
        Particle[] allParticles = FindObjectsOfType<Particle>();
        for (int i = 0; i < allParticles.Length; i++)
        {
            //check for Particles
            Particle testParticle = allParticles[i];

            //if testParticle is not already part of the atom, is not attached to anything, and is within attractionDistance
            if (!particles.Contains(testParticle) && (!allParticles[i].attached || allParticles[i].type == Particle.ParticleType.Electron) && Vector3.Distance(testParticle.transform.position,this.transform.position) < attractionDistance)
            {
                if (particles.Count < maxParticles)
                {
                    particles.Add(testParticle);
                }
            }
        }

        protonAmount = 0;
        neutronAmount = 0;
        electronAmount = 0;
        //check particles attached to atom
        for (int i = 0; i < particles.Count; i++)
        {
            //checks if distance is less than int and makes sure the particle is not an electron then parents the particle to the atom
            if (Vector3.Distance(particles[i].transform.position, this.transform.position) < 1 && particles[i].type != Particle.ParticleType.Electron)
            {
                particles[i].attached = true;
                //particles[i].transform.SetParent(this.transform);
            }

            //checks if the particle is beyond the cutoff point and if it is, removes it
            if (Vector3.Distance(particles[i].transform.position, this.transform.position) > 50 &&
                particles[i].type != Particle.ParticleType.Electron)
            {
                particles[i].attached = false;
                particles.Remove(particles[i]);
            }

            //counts each type of particle
            if (particles[i].type == Particle.ParticleType.Electron) {
                electronAmount++;
            }
            else if (particles[i].type == Particle.ParticleType.Proton) {
                protonAmount++;
            }
            else if (particles[i].type == Particle.ParticleType.Neutron) {
                neutronAmount++;
            }
        }

        //claculate bias
        CalculateBias();
        Debug.Log("protons: " + bias[0] + " neutrons: " + bias[1] + " electrons: " + bias[2]);
    }

    public void ForceOnAtomParticles()
    {
        foreach (Particle particle in particles)
        {
            //get the rigidbody so long as its valid and not parented 
            if (particle.gameObject.GetComponent<Rigidbody>()!=null && particle.transform.parent == null)
            {

                Rigidbody rb = particle.gameObject.GetComponent<Rigidbody>();
                float distance = Vector3.Distance(rb.position,this.transform.position);
                Vector3 force = new Vector3(0,0,0);
                

                switch (particle.type)
                {
                    case Particle.ParticleType.Proton:
                        force = (this.transform.position - rb.transform.position) * rb.mass  * bias[0] * 10;
                        break;
                    case Particle.ParticleType.Neutron:
                        force = (this.transform.position - rb.transform.position) * rb.mass * bias[1] * 10;
                        break;
                    case Particle.ParticleType.Electron:

                        //creates an offset that circles around the atom center at the speed of time
                        float loopOffset = (float) 360 *( (float) (electronSinOffset) / (float)electronAmount);

                        Vector3 offset = new Vector3(15 * Mathf.Cos(loopOffset + Time.time), 0, 15 * Mathf.Sin(loopOffset + Time.time) );

                        force = (this.transform.position - rb.transform.position + offset) * rb.mass * bias[2] * 10;

                        electronSinOffset++;
                        if (electronSinOffset > electronAmount-1)
                        {
                            electronSinOffset = 0;
                        }

                        break;
                }

                rb.AddForce(force);
            }
        }
    }

    public void DestroyAtom()
    {
        //like restart atom but more dramatic
        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[0] != null) 
            {
                Rigidbody rb = particles[0].GetComponent<Rigidbody>();
                particles[0].attached = false;
                particles[0].transform.SetParent(null);
                rb.AddExplosionForce(10000, rb.position, 10000);
                particles.Remove(particles[0]);
            }
        }
        particles.Clear();
    }

    public void RestartAtom() 
    {
        //goes through each list and so long as the particle is valid, sets the attachment to false, sets the parent to null, and removes the particle from the list
        for (int i = 0; i < particles.Count; i++) 
        {
            if (particles[0] != null) 
            {
                particles[0].attached = false;
                particles[0].transform.SetParent(null);
                particles.Remove(particles[0]);
            }
        }
        particles.Clear();
    }

    public void EjectParticle()
    {
        //find a random particle to eject, maybe something biased against
        CalculateBias();
    }

    public void WatchParticleStats()
    {
        //check to see if the number of particles exceeds the 
        if (particles.Count> maxParticles)
        {
            EjectParticle();
        }
    }






    void Start()
    {
        RestartAtom();
        CheckParticle();
        Debug.Log("There are: "+particles.Count+" attached to this atom.");

    }

    void Update()
    {
        if (Time.fixedTime % secondsBetweenUpdates == 0)
        {
            CheckParticle();
            WatchParticleStats();

        }
        
    }

    void FixedUpdate()
    {
        ForceOnAtomParticles();
        if (Time.time % 60 == 0)
        {
            RestartAtom();
        }
    }
}
