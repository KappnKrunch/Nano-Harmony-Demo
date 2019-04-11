using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

[System.Serializable]
public class WeightsBiases : MonoBehaviour
{
    float[] weight = new float[3]; //(proton, neutron, electron)  how hard it is to pull for n type of particle; 1 is the max weight and makes the particle very hard to pull; -1 will make the atom go towards it
    float[] bias = new float[3]; //(proton, neutron, electron) how strong the particle pulls on n type of particle; 1 is very strong; -1 pushes 

    public enum ParticleType { Proton, Neutron, Electron }
    private ParticleType type = ParticleType.Proton;

    private float maxDistance = 12;

    public float GetMaxDistane()
    {
        return this.maxDistance;
    }

    public void SetMaxDistance()
    {

    }


    public void SetWeight(float[] newWeight) {
        //if its greater or less than negative 1
        if ((newWeight[0] + newWeight[1] + newWeight[2])  / 3 <= 1 || 
            (newWeight[0] + newWeight[1] + newWeight[2]) / 3 >= -1 ||
            newWeight.Length > 3 || newWeight.Length < 3 )
        {
            this.weight = newWeight;
        }
    }

    public float[] GetWeight()
    {
        return this.weight;
    }

    public void SetBias(float[] newBias) {
        if ((newBias[0] + newBias[1] + newBias[2]) / 3 <= 1 ||
            (newBias[0] + newBias[1] + newBias[2]) / 3 >= -1 ||
            newBias.Length > 3 || newBias.Length < 3) 
        {

            this.bias = newBias;
        }
    }

    public float[] GetBias()
    {
        return this.bias;
    }

    public void SetParticleType(ParticleType newType)
    {
        this.type = newType;
    }

    public ParticleType GetParticleType()
    {
        return this.type;
    }
}
