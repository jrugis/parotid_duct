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
    private int idx_vars;            // index into ion concentrations for this disc
    private GameObject duct;
    public float c;

    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
        duct = GameObject.Find("virtual_duct");
        var diameter = mini_gland.GetComponent<mini_gland_properties>().disc_diameters[disc_id];
        area = Mathf.PI * diameter * diameter / 4.0F; // disc cross-section area
        flow = mini_gland.GetComponent<mini_gland_properties>().dyn_data[disc_id];  // initialise to first value
        int nvars = mini_gland.GetComponent<mini_gland_properties>().ndvars;
        idx_vars = mini_gland.GetComponent<mini_gland_properties>().disc_idx + (nvars * disc_id);
    }

    void Update()
    {
        //if (!mini_gland.GetComponent<toggle_sim>().simulate) return;
        var idx = duct.GetComponent<duct_properties>().display_state;

        // visualise flow velocity
        flow = mini_gland.GetComponent<mini_gland_properties>().dyn_data[disc_id];
        velocity = flow / area;
        var dx = velocity * Time.deltaTime;
        dx *= 4; // a bit faster for visulisation
        Material mat = GetComponent<Renderer>().material;
        float x = mat.mainTextureOffset.x;
        float y = mat.mainTextureOffset.y + (dx / 1024); // scaled by texture size
        mat.mainTextureOffset = new Vector2(x, y);

        // visualise an ion concentration
        if (idx == -1) idx = 0;  // index -1 is for "not visible" state so don't use it here!
        var props = duct.GetComponent<duct_properties>().ion_props[idx];
        c = mini_gland.GetComponent<mini_gland_properties>().dyn_data[idx_vars + idx];
        mat.color = Color.Lerp(Color.green, Color.red, (c - props.min) / (props.max - props.min));
    }
}
