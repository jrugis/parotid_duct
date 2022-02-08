/*
Parotid Simulation 
Attach this to an object.
*/
using System;
using System.Text;
using UnityEngine;

public class spin: MonoBehaviour
{
    void Start()
    {
    }
    void Update ()
    {
        if (Input.GetKey (KeyCode.LeftShift)) return;  
        if (Input.GetMouseButton(0))
        {
            float multiplier = 5.0F;
            float h = multiplier * Input.GetAxis("Mouse X");
            float v = multiplier * Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler(v, -h, 0) * transform.localRotation;
        }
    }  
}