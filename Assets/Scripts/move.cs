/*
Attach this to a Camera object.
*/
using System;
using System.Text;
using UnityEngine;

public class move: MonoBehaviour
{
    void Update ()
    {
        if (Input.GetKey (KeyCode.LeftShift) & Input.GetMouseButton(0))
        {
            float v = 0.5F * Input.GetAxis("Mouse Y");
        }
        if (Input.GetMouseButton(1))
        {
            float h = 0.05F * Input.GetAxis("Mouse X");
            float v = 0.05F * Input.GetAxis("Mouse Y");
            if (Input.GetKey (KeyCode.LeftShift)) transform.localPosition += 5 * v * Vector3.back;
            else transform.localPosition += h * Vector3.left + v * Vector3.down;;
        }
    }  
}