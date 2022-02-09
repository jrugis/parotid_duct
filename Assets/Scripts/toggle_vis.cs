/*
Parotid Simulation 
Attach this to a cells prefab object.
*/
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class toggle_vis : MonoBehaviour
{
    Dictionary<char, KeyCode> chartoKeycode = new Dictionary<char, KeyCode>()
    {
        {'i', KeyCode.I},        // intercalated cells
        {'s', KeyCode.S},        // striated cells
        {'1', KeyCode.Alpha1},   // acinus cells
        {'2', KeyCode.Alpha2},   // ...
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
        char c = this.name[0];            // get the first letter of the object's name,
        if (c == 'a') c = this.name[1];   //   but for acinii, get the second letter  
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