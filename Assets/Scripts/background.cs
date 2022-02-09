/*
Parotid Simulation 
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
         new Color(0.0f, 0.0f, 0.0f, 1.0f),
         new Color(0.15f, 0.15f, 0.15f, 1.0f),
         new Color(0.3f, 0.3f, 0.3f, 1.0f),
         new Color(0.68f, 0.68f, 0.68f, 1.0f),
         new Color(1.0f, 1.0f, 1.0f, 1.0f)
    };
    // Start is called before the first frame update
    void Start()
    {
    }
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
