using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RippleEffect : MonoBehaviour
{
    public float maxDistance = 10;
    public float frequency = 2;
    private float distance = 0;

    public Material rippleFloor;
    public Material electronMaterial;
    private Vector4[] origins;

    // Start is called before the first frame update
    void Start()
    {
    }

    void GetHeight()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
