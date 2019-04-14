using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RippleEffect : MonoBehaviour
{
    public float maxDistance = 10;
    public float frequency = 2;
    public GameObject atom;
    private float distance = 0;

    public Material material;

    public Floor floor;

    // Start is called before the first frame update
    void Start()
    {
        //atoms = floor.atoms;
    }

    void GetHeight()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
         
        float time = 18 + (6 * Mathf.Sin(Time.fixedTime));
        distance = time;
        //Debug.Log(distance);
        material.SetFloat("_RippleDistance",distance);
        material.SetVector("_RippleOrigin",atom.transform.position);
        //aterial.SetFloat("_RippleWidth",maxDistance);

    }
}
