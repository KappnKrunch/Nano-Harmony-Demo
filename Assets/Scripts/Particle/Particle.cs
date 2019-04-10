using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
[RequireComponent(typeof(Rigidbody))]
public class Particle : MonoBehaviour
{
    //defines the type of particlle and set the default to Proton
    public enum ParticleType { Proton, Neutron, Electron};
    public ParticleType type = ParticleType.Proton;

    //these are the prefabs for the different particles
    public GameObject proton;
    public GameObject neutron;
    public GameObject electron;
    public bool detach = false;

    private Rigidbody rb;
    private GameObject particle;
    public bool attached = false;

    void Particel(ParticleType newType)
    {

        switch (newType)
        {
            case ParticleType.Proton:
                particle = Instantiate(proton, transform);
                break;

            case ParticleType.Neutron:
                particle = Instantiate(neutron, transform);
                break;

            case ParticleType.Electron:
                particle = Instantiate(electron, transform);
                break;

            default:
                particle = Instantiate(proton, transform);
                break;
        }
    }

    public void ResetParticle() 
    {
        while (transform.childCount > 0) 
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void SetType(ParticleType newType) 
    {
        particle = null;
        ResetParticle();

        Particel(newType);

    }

    private ParticleType oldType;
    public void WatchIfTypeChanged()
    {
        if (oldType != type)
        {
            SetType(type);
            oldType = type;
        }
    }

    public void Detach()
    {
        //broken
        //detaches the particle with force

        Debug.Log(transform.parent);
        if (transform.parent != null)
        {
            Debug.Log("yup");
            Vector3 explosionoPos = transform.parent.transform.position; // saves the old parent postion

            Atom parent = transform.GetComponentInParent<Atom>();
            parent.particles.Remove(this);//remove the particle from the parents particle list

            transform.SetParent(null); //unparents the particle from the atom

            rb.AddExplosionForce(100000000000,explosionoPos,10);
        }
    }


    void ApplyForce()
    {
        float distancStrength = 0;

        Vector3 noise = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Vector3 force = Vector3.zero;
        //Debug.Log(noise);

        switch (type) 
        {
            case ParticleType.Proton:
                //Protons are attracted to neutrons and atom centers


                break;

            case ParticleType.Neutron:
                //neutrons are attracted to atoms

                break;

            case ParticleType.Electron:
                
                break;
                //electrons are repelled by everything but high points in the energy field (x,y) coords

            default:
                
                break;
        }

        rb.AddForce(force);
    }

    void Update()
    {

    }

    void OnEnable() 
    {
        rb = gameObject.GetComponent<Rigidbody>();
        SetType(type);
    }

    void OnUpdate() 
    {
        if (detach) 
        {
            Detach();
        }

        WatchIfTypeChanged();
        
    }

    void FixedUpdate()
    {
        //ApplyForce();
    }

}
