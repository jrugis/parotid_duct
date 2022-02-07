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
        if (Input.GetKey (KeyCode.LeftArrow))
        {  
            transform.RotateAround(Vector3.zero, Vector3.up, 0.5F);
        }      
        else if (Input.GetKey (KeyCode.RightArrow))
        {  
            transform.RotateAround(Vector3.zero, Vector3.up, -0.5F);
        }
        if (Input.GetKey (KeyCode.UpArrow))
        {  
            transform.RotateAround(Vector3.zero, Vector3.right, 0.5F);
        }      
        else if (Input.GetKey (KeyCode.DownArrow))
        {  
            transform.RotateAround(Vector3.zero, Vector3.right, -0.5F);
        }
        // Get the mouse delta. This is not in the range -1...1
        //float h = 2.0F * Input.GetAxis("Mouse X");
        //float v = 2.0F * Input.GetAxis("Mouse Y");
        //transform.Rotate(v, h, 0);
 
    }  
}