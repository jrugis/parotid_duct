/*
Attach this to a MiniGland object.
*/
using System;
using System.Text;
using UnityEngine;

public class toggle_sim : MonoBehaviour
{
    public bool simulate;

    void Start()
    {
        simulate = false;
    }

    void Update ()
    {  
        if (Input.GetKeyDown (KeyCode.Space)) simulate = !simulate;
    }  
}
