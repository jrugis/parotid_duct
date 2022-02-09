/*
Parotid Simulation 
Attach this to a MiniGland object.
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
            transform.RotateAround(Vector3.zero, Vector3.up, -5.0F * Input.GetAxis("Mouse X"));
            transform.RotateAround(Vector3.zero, Vector3.right, 5.0F * Input.GetAxis("Mouse Y"));
        }
    }  
}