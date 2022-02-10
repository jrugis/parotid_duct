/*
Parotid Simulation 
Attach this to a MiniGland object.
*/
using System;
using System.Text;
using UnityEngine;

public class toggle_rotate : MonoBehaviour
{
    void Start()
    {
    }

    void Update ()
    {  
        if (Input.GetKeyDown (KeyCode.R))
        {  
            GetComponent<rotate>().enabled = !GetComponent<rotate>().enabled;
        }      
    }  
}