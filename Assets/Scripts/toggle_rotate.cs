using System;
using System.Text;
using UnityEngine;

public class toggle_rotate : MonoBehaviour
{
    void Update ()
    {  
        if (Input.GetKeyDown (KeyCode.R))
        {  
            GetComponent<rotate>().enabled = !GetComponent<rotate>().enabled;
        }      
    }  
}