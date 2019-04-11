using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float forceMutliplier = 1;

    //lists of all the particles and what they are
    public List<WeightsBiases> particles = new List<WeightsBiases>(); // set to private once finished debugging


    void CheckParticleUpdate()
    {
        //looks for any particles and sets the list to those particles
        particles = FindObjectsOfType<WeightsBiases>().ToList();
    }

    void ApplyForceToParticles()
    {
        //this function goes through the particles list and uses the weights and biases to apply force to earch of the particles
        //it doesn't know what type each paticle is but it doesn't matter. It only needs the weights and biases to apply force to the rigidbody

        //every particle in the list
        for (int i = 0; i < particles.Count; i++)
        {
            Rigidbody mainParticle = particles[i].GetComponent<Rigidbody>();
            Vector3 howMuchItsPulled = Vector3.zero;

            //is going to effect every other particle in the list
            for (int j = 0; j < particles.Count; j++)
            {
                Rigidbody secondaryParticle = particles[i].GetComponent<Rigidbody>();
                float distance = Vector3.Distance(mainParticle.position, secondaryParticle.position);

                howMuchItsPulled += (secondaryParticle.position - mainParticle.position);

            }

            mainParticle.AddForce(howMuchItsPulled); // the particle is pulled by howMuchItsPulled

        }
    }

}
