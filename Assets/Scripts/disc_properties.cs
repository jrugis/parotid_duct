using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disc_properties : MonoBehaviour
{
    public int disc_id; 
    public float flow;
    public float velocity;
    public float area;
    private GameObject mini_gland;   // for quick reference

    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
        var diameter = mini_gland.GetComponent<mini_gland_properties>().disc_diameters[disc_id];
        area = Mathf.PI * diameter * diameter / 4.0F; // disc cross-section area
        flow = mini_gland.GetComponent<mini_gland_properties>().dyn_data[disc_id];  // initialise to first value
    }

    void Update()
    {
        if (!mini_gland.GetComponent<toggle_sim>().simulate) return;
        flow = mini_gland.GetComponent<mini_gland_properties>().dyn_data[disc_id];
        velocity = flow / area;
        var dx = velocity * Time.deltaTime;
        dx *= 4; // a bit faster for visulisation

        // visualise flow velocity
        Material mat = GetComponent<Renderer>().material;
        float x = mat.mainTextureOffset.x;
        float y = mat.mainTextureOffset.y + (dx / 1024); // scaled by texture size
        mat.mainTextureOffset = new Vector2(x, y);
    }
}
