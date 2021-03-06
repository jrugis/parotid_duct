/*
Attach this to a Camera object.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    int color_state = 0;
    Color[] bcolors = 
    {
         new Color(0.0F, 0.0F, 0.0F, 1.0f),
         new Color(0.15F, 0.15F, 0.15F, 1.0f),
         new Color(0.3F, 0.3F, 0.3F, 1.0f),
         new Color(0.68F, 0.68F, 0.68F, 1.0f),
         new Color(1.0F, 1.0F, 1.0F, 1.0f)
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.B))
        {
            if (++color_state >= bcolors.Length) color_state = 0;
            GetComponent<Camera>().backgroundColor = bcolors[color_state];
        }
    }
}
