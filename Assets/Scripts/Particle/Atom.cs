using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Atom : MonoBehaviour
{
    public Vector3 bias = new Vector3(1,1,1); //bias should always be between
    public bool automaticallyUpdateBias = false;
    public List<GameObject> particlesAttachedToAtom = new List<GameObject>();

    public void DestroyAtom()
    {
        //deletes the atom empty and seperates each particle
    }

    public void ResetAtom()
    {
        //restores lists but doesnt remove atom
    }

}
