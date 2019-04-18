using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Atom : MonoBehaviour
{
    public bool explode = false;
    public float[] atomBias = new float[3] {1, 1, 1};
    public float[] atomCount = new float[3] {0,0,0};
    public float maxDistance = 30;
    public float radius = 20;
    public float timeMultiplier = 1;
    public float mass = 0;
    [Range(0, 8)] public int octaves;
    [Range(0, 10)] public float noiseRange;
    public Vector2 ratio = new Vector2(0,0);
    public Vector3 massCenter;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public GameObject mainFloor;
    private Floor floor;
    public List<WeightsBiases> particles = new List<WeightsBiases>();
    private SinewaveExample atomSounds;

    void CalculateMassCenter()
    {
        //totals all the atoms transforms together and aaverages it by the total nummber.
        // the final number is the mass center
        Vector3 totalPoints = Vector3.zero;
        for (int i = 0; i < particles.Count; i++)
        {
            totalPoints += particles[i].transform.position;
        }

        massCenter = totalPoints / particles.Count;
    }

    public void CalculateBias()
    {
        mass = (int)(atomCount[0] + atomCount[1] + (atomCount[2] / Mathf.Clamp(atomCount[0], 1, 1000)) );
        if (mass > 0)
        {
            atomBias[0] = 0.875f - (atomCount[0] / mass);
            atomBias[1] = 0.875f - (atomCount[1] / mass);
            atomBias[2] = 0.875f - ( (atomCount[2] / Mathf.Clamp(atomCount[0],1,1000) )  / mass);

            ratio.x = (int) (atomCount[0] / 12);
            ratio.y = (int) (atomCount[1] / 12);
            //Debug.Log("Total mass: "+mass+" protons bias: " + atomBias[0] + " neutron bias: " + atomBias[1] + " electron bias: " + atomBias[2]);
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
                float inverseDistance = Mathf.Clamp((maxDistance - Vector3.Distance(particle.position, this.transform.position)) / maxDistance, 0, 1);

                if (particles[i].GetParticleType() == WeightsBiases.ParticleType.Electron) {
                    //offset for electron circlular rotation

                    float electronFlowTime = (electronIndex / atomCount[2] * 2 * Mathf.PI);
                    
                    Vector3 offset =
                        new Vector3(
                            radius * Mathf.Sin( (timeMultiplier * Time.time) + electronFlowTime), 0,
                            radius * Mathf.Cos( (timeMultiplier * Time.time) + electronFlowTime) 
                    );

                    //rotates the offset
                    Quaternion rotation = Quaternion.Euler(
                        45 * Mathf.Cos( (atomCount[0] % 12 +1) * electronFlowTime) , 0, 
                        45 * Mathf.Sin( (atomCount[1] % 12 +1) * electronFlowTime) 
                    );

                    offset = rotation * offset;

                    force = (this.transform.position - particle.position + offset) * particle.mass * inverseDistance * atomBias[(int)particles[i].type] * mass;//10 is a stability factor
                    electronIndex++; //cycle through the electrons
                }
                else 
                {
                    force = (this.transform.position - particle.position) * particle.mass * inverseDistance * atomBias[(int)particles[i].type] * 10; //leave 10 for stability
                }

                particle.AddForce(force);
            }
        }
    }

    public void DestroyAtom()
    {
        //does what it says

        for (int i = 0; i < particles.Count; i++)
        {
            Rigidbody rb = particles[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(1000000,this.transform.position,40);
            Destroy(this.gameObject);
        }

        atomCount[0] = 0;
        atomCount[1] = 0;
        atomCount[2] = 0;
        particles.Clear();
        maxDistance = 0;
        Destroy(this.gameObject);
        Debug.Log("BOOM");
    }

    void DestroyAtomButton()
    {
        if ((int)Time.time % 15 == 0 && (int)Time.time != 0 && explode)
        {
            DestroyAtom();
        }
    }

    public float GetMass() 
    {
        return mass;
    }

    public void UpdateAtomSounds()
    {
        atomSounds.frequency1 = (int)atomCount[0] % 12;
        atomSounds.frequency2 = (int)atomCount[1] % 12;
        atomSounds.octaves = 1 + (int) (mass / 50);
        atomSounds.noise = UnityEngine.Random.Range(-noiseRange / 10, noiseRange / 10);
       
    }

    void Start()
    {
        floor = GameObject.FindGameObjectWithTag("floor").GetComponent<Floor>();
        particles.Clear();
        atomSounds = this.GetComponent<SinewaveExample>();
    }

    void Update()
    {
        FindParticlesNearAtom();
        DestroyAtomButton();
        UpdateAtomSounds();


    }

    void FixedUpdate()
    {
        CalculateMassCenter();
        ForceParticlesAttachedToAtom();
    }

}
