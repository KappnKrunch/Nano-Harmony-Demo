using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour
{
    //public float forceMutliplier = 0f;

    //lists of all the particles and what they are
    public List<WeightsBiases> particles = new List<WeightsBiases>(); // set to private once finished debugging
    public List<Atom> atoms = new List<Atom>();
    public Material floorMaterial;
    public Material electronMaterial;
    private Vector4[] origins = new Vector4[10];

    void CheckParticleUpdate()
    {
        //looks for any particles and sets the list to those particles
        particles.Clear();
        particles = FindObjectsOfType<WeightsBiases>().ToList();
        atoms = FindObjectsOfType<Atom>().ToList();

    }

    void AnimateAtomRipples()
    {
        for (int i = 0; i < atoms.Count; i++)
        {
            if (atoms[i] != null)
            {
                float distance = ((atoms[i].radius + 5) * (8.0f/9)) + ( (atoms[i].radius + 5) * Mathf.Sin(Time.fixedTime) * (1.0f/9) );

                origins[i] = atoms[i].transform.position;
                origins[i].w = distance;
            }
        }   

        floorMaterial.SetInt("_PointsCount", atoms.Count);
        floorMaterial.SetVectorArray("_Points", origins);

        electronMaterial.SetInt("_PointsCount", atoms.Count);
        electronMaterial.SetVectorArray("_Points", origins);
    }

    void CheckAtomsDistance() {
        //checks each atoms distance from each other and if its too close 
        //destroys the atom with the lesser mass

        for (int i = 0; i < atoms.Count; i++) 
        {
            for (int j = 0; j < atoms.Count; j++) 
            {
                float minDistance = atoms[i].maxDistance - (atoms[j].maxDistance / 2);
                if (Vector3.Distance(atoms[i].massCenter, atoms[j].massCenter) <= minDistance && atoms[i] != atoms[j]) 
                {
                    if (atoms[i].GetMass() > atoms[j].GetMass()) 
                    {
                        Destroy(atoms[j].gameObject);
                        atoms.Remove(atoms[j]);
                        
                    }
                    else {
                        Destroy(atoms[i].gameObject);
                        atoms.Remove(atoms[i]);
                        
                    }
                }
            }
        }
    }

    void Start()
    {
        particles.Clear();
    }

    void Update()
    {

        CheckParticleUpdate();
    }

    void FixedUpdate()
    {
       // ApplyForceToParticles();
       AnimateAtomRipples();
       CheckAtomsDistance();
    }
}
