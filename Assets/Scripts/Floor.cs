using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float forceMutliplier = 0f;
    public int updateSpeed = 3;

    //lists of all the particles and what they are
    public List<WeightsBiases> particles = new List<WeightsBiases>(); // set to private once finished debugging
    public List<Atom> atoms = new List<Atom>();


    void CheckParticleUpdate()
    {
        //looks for any particles and sets the list to those particles
        particles.Clear();
        particles = FindObjectsOfType<WeightsBiases>().ToList();
        atoms = FindObjectsOfType<Atom>().ToList();

    }

    void ApplyForceToParticles()
    {
        //this function goes through the particles list and uses the weights and biases to apply force to earch of the particles
        //it doesn't know what type each paticle is but it doesn't matter. It only needs the weights and biases to apply force to the rigidbody

        //every particle in the list
        for (int i = 0; i < particles.Count; i++)
        {
            Rigidbody mainParticle = particles[i].GetComponent<Rigidbody>(); //gets pulled on by every particle
            Vector3 mainParticleForce = new Vector3(0,0,0);
            int forced = 0; //how many particles are in range so that it can be averaged

            //is going to effect every other particle in the list
            for (int j = 0; j < particles.Count; j++)
            {
                Rigidbody secondaryParticle = particles[i].GetComponent<Rigidbody>(); //gets pulled on by mainParticle

                float distance = Vector3.Distance(mainParticle.position, secondaryParticle.position);

                if (distance <= particles[i].GetMaxDistane() || distance <= particles[j].GetMaxDistane())
                {



                    float inverseDistanceFromSecondary = Mathf.Clamp(particles[j].GetMaxDistane() - distance, 0, particles[j].GetMaxDistane());
                    float inverseDistanceFromMain = Mathf.Clamp(particles[i].GetMaxDistane() - distance, 0, particles[i].GetMaxDistane() );


                    mainParticleForce += ((secondaryParticle.position - mainParticle.position) *
                                          particles[j].GetBias()[(int) particles[j].GetParticleType()]) -
                                         (new Vector3(500,500,500) *particles[i].GetWeight()[(int) particles[i].GetParticleType()] )*
                                         inverseDistanceFromSecondary;
                        

                    Vector3 secondaryParticleForce = ((secondaryParticle.position - mainParticle.position) *
                                                      particles[i].GetBias()[(int)particles[i].GetParticleType()]) -
                                                     (new Vector3(500, 500, 500) * particles[j].GetWeight()[(int)particles[j].GetParticleType()]) *
                                                     inverseDistanceFromMain;

                    secondaryParticleForce = secondaryParticleForce * forceMutliplier;

                    secondaryParticle.AddForce(secondaryParticleForce );
                    forced++;
                }
            }
            //Debug.Log(mainParticleForce);
            mainParticleForce = ( mainParticleForce / forced) * forceMutliplier;

            mainParticle.AddForce( mainParticleForce); // how much the main particle is pulled / mow many particles there are

        }
    }

    void Start()
    {
        particles.Clear();
    }

    void Update()
    {
        //Debug.Log((int) Time.time % updateSpeed == 0);
        if ((int)Time.time % updateSpeed == 0)
        {
            CheckParticleUpdate();
            
        }
    }

    void FixedUpdate()
    {
       // ApplyForceToParticles();
    }
}
