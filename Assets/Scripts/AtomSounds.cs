using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSounds : MonoBehaviour
{
    private List<Atom> atoms;

    public float CreateSine(int timeIndex, float frequency, float sampleRate) 
    {
        float total = 0;
        for (int i = 0; i < 4; i++) 
        {
            total += Mathf.Sin(Mathf.Pow(2, i + 1) * Mathf.PI * timeIndex * frequency / sampleRate) / Mathf.Pow(i, i);
            //total += Mathf.Sin(Mathf.Pow(2, i + 7) * Mathf.PI * timeIndex * frequency / sampleRate) / Mathf.Pow(i, i);
        }

        return total;
    }

    void GenerateSoundsFromAtoms()
    {
        for (int i = 0; i < atoms.Count; i++)
        {

        }
    }

    void Start()
    {
        this.atoms = this.GetComponent<Floor>().atoms;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
