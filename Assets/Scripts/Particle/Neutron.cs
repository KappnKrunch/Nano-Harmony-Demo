using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Neutron : MonoBehaviour
{
    private WeightsBiases weightsBiases;
    Vector3 weight = new Vector3(0.8f, 0, 1); //(proton, neutron, electron)  how hard it is to pull n type of particle; 1 is the max weight and makes the particle very hard to pull; -1 will make the atom go towards it
    Vector3 bias = new Vector3(0.3f, 0, 0.4f); //(proton, neutron, electron) how strong the particle pulls on n type of particle; 1 is very strong; -1 pushes away

    void Init() 
    {
        weightsBiases.SetWeight(weight);
        weightsBiases.SetBias(bias);
    }

    void Start() 
    {
        weightsBiases = this.GetComponent<WeightsBiases>();
    }

}
