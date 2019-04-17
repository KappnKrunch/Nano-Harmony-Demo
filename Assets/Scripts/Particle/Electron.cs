using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Electron : MonoBehaviour
{

    //detect collisions
    //change particle effects

    private WeightsBiases weightsBiases;
    readonly float[] weight = new float[3] { 0.1f, 0.1f, -0.3f}; //(proton, neutron, electron)  how hard it is to pull n type of particle; 1 is the max weight and makes the particle very hard to pull; -1 will make the atom go towards it
    readonly float[] bias = new float[3] { 0.1f, 0.1f, 0}; //(proton, neutron, electron) how strong the particle pulls on n type of particle; 1 is very strong; -1 pushes away
    private ParticleSystem particleSystem;
    private Rigidbody rigidbody;

    void Init()
    {
        weightsBiases = this.GetComponent<WeightsBiases>();
        weightsBiases.SetWeight(weight);
        weightsBiases.SetBias(bias);
        weightsBiases.SetParticleType(WeightsBiases.ParticleType.Electron);
        
        particleSystem = this.GetComponent<ParticleSystem>();
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void ParticleDirecction()
    {

        //particleSystem.inheritVelocity.mode
    }

    void ParticleCheck()
    {
        if (rigidbody.maxAngularVelocity == 0)
        {
            particleSystem.Pause();
        }
        else
        {
            particleSystem.Pause();
        }
    }

    void Start() 
    {
        Init();
    }

    void FixedUpdate()
    {
        ParticleCheck();
    }


    

}
