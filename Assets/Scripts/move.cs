/*
Parotid Simulation 
Attach this to a Camera object.
*/
using System;
using System.Text;
using UnityEngine;

public class move: MonoBehaviour
{
    void Start()
    {
    }
    void Update ()
    {
        if (!Input.GetKey (KeyCode.LeftShift)) return;  
        if (Input.GetMouseButton(0))
        {
            float v = 0.5F * Input.GetAxis("Mouse Y");
            transform.localPosition += v * Vector3.back;
        }
    }  
}