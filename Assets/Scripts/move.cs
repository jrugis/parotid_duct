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
        if (Input.GetKeyDown (KeyCode.C))
        {  
            transform.localPosition /= 1.1F; // move in
        }      
        else if (Input.GetKeyDown (KeyCode.F))
        {  
            transform.localPosition *= 1.1F; // move out
        }
    }  
}