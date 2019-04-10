using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject particleTemplate;
    public GameObject atomTemplate;
    List<GameObject> particles = new  List<GameObject>();
    List<GameObject> atoms = new List<GameObject>();


    void SpawnRandomParticles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast( new Vector3(Random.Range(0, 100), 1000, Random.Range(0, 100)),Vector3.down, out hit, 1100) )
            {
                GameObject newParticle = Instantiate(particleTemplate, hit.point, Quaternion.identity);
                particles.Add(newParticle);
            }
        }
    }

    void SpawnRandomAtoms(int amount) {
        for (int i = 0; i < amount; i++) {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(Random.Range(0, 100), 1000, Random.Range(0, 100)), Vector3.down, out hit, 1100)) {
                GameObject newAtom = Instantiate(particleTemplate, hit.point, Quaternion.identity);
                atoms.Add(newAtom);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
       // SpawnRandomParticles(15);
       // SpawnRandomAtoms(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
