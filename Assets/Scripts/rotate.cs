/*
Parotid Simulation 
Attach this to a MiniGland object.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    // Use this for initialization
    void Start () 
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local Y axis at 1 degree per second
        transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
    }
}