/*
Parotid Simulation 
Attach this to help text object.
*/
using System;
using System.Text;
using UnityEngine;

public class toggle_help : MonoBehaviour
{
    void Start()
    {
    }
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.H))
        {  
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
        }      
    }  
}