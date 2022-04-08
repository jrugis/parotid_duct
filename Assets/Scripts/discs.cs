using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class discs : MonoBehaviour
{
    public GameObject disc;           // the object to clone
    void Start()   
    {
        // get terse references to relevant data from MiniGland object
        var mini_gland = GameObject.Find("MiniGland");
        var ndiscs = mini_gland.GetComponent<mini_gland_properties>().ndiscs;
        var tsteps = mini_gland.GetComponent<mini_gland_properties>().tsteps;
        var disc_centers = mini_gland.GetComponent<mini_gland_properties>().disc_centers;
        var disc_dirs = mini_gland.GetComponent<mini_gland_properties>().disc_dirs;
        var disc_diameters = mini_gland.GetComponent<mini_gland_properties>().disc_diameters;
        var dyn_data = mini_gland.GetComponent<mini_gland_properties>().dyn_data;

        // create the disc objects
        float scale = 0.01F; 
        for (int d = 0; d < ndiscs; d++)
        {
            GameObject obj = Instantiate(disc);
            obj.GetComponent<disc_properties>().disc_id = d; // set disc id 
            obj.transform.SetParent(this.transform);          // place in the hierarchy
 
            //set position and orientation
            float s = disc_diameters[d]*scale; 
            obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(s,s,s));
            obj.transform.LookAt(disc_dirs[d]);
            obj.transform.Rotate(90,0,0);
            obj.transform.position += scale * disc_centers[d] + new Vector3(0F,0.30F,0F); // FIX Y-OFFSET ????
 
            // randomly offset the texture x-position so that adjecent textures don't align
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material.mainTextureOffset = new Vector2(UnityEngine.Random.Range(0, 1F), UnityEngine.Random.Range(0, 1F));
            rend.enabled = false;  // start hidden
        }
    }
}
