using System;
using System.Text;
using UnityEngine;

public class toggle_visibility : MonoBehaviour
{
    public KeyCode kcode; // set this in each object's GUI
    void Update ()
    {  
        if (Input.GetKeyDown (kcode))
        {  
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }      
    }  
}