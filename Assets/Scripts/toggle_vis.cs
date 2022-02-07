/*
Parotid Simulation 
Attach this to an object.
*/
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class toggle_vis : MonoBehaviour
{
    Dictionary<char, KeyCode> chartoKeycode = new Dictionary<char, KeyCode>()
    {
        {'i', KeyCode.I},
        {'s', KeyCode.S},
        {'1', KeyCode.Alpha1},
        {'2', KeyCode.Alpha2},
        {'3', KeyCode.Alpha3},
        {'4', KeyCode.Alpha4},
        {'5', KeyCode.Alpha5},
        {'6', KeyCode.Alpha6}
    };
    void Start()
    {
    }

    void Update ()
    {
        KeyCode kcode;
        char c = this.name[0];
        if (c == 'a') c = this.name[1];

        if (chartoKeycode.TryGetValue(c, out kcode))
        {
            if (Input.GetKeyDown (kcode))
            {
                Renderer[] renders = GetComponentsInChildren<Renderer>(); 
                foreach (Renderer rend in renders) rend.enabled = !rend.enabled;
            }
        }



    }
}