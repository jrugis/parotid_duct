using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct ions
{
    public string name; 
    public float min;
    public float max;
}

public class duct_properties : MonoBehaviour
{
    private GameObject mini_gland;
    private Transform fMin_ref;
    public Text fBG;  // 
    public Text fText;          // for quick reference
    public RawImage fColorBar;  // 
    public RawImage fMin_image;
    public Transform fMin_transform;
    public RawImage fMax_image;
    public Transform fMax_transform;
    public List<string> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    public float[] dyn_data;
    public float[] c;
    public int idx_vars;
    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
        fMin_ref = GameObject.Find("min_fluid_ref").GetComponent<Transform>();
        fText = GameObject.Find("fluid_display").GetComponent<Text>();
        fBG = GameObject.Find("fluid_bg").GetComponent<Text>();
        fColorBar = GameObject.Find("color_bar").GetComponent<RawImage>();
        fMin_image = GameObject.Find("min_fluid").GetComponent<RawImage>();
        fMin_transform = GameObject.Find("min_fluid").GetComponent<Transform>();
        fMax_image = GameObject.Find("max_fluid").GetComponent<RawImage>();
        fMax_transform = GameObject.Find("max_fluid").GetComponent<Transform>();
        ion_props = new List<string>(){"Na", "K", "Cl", "HCO3", "pH"};
    }
    void Update()
    {
        float min, max;
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1;
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renders) rend.enabled = (display_state != -1);
            fText.enabled = (display_state != -1);  // index -1 is for "not visible" state
            fColorBar.enabled = fMin_image.enabled = fMax_image.enabled = fBG.enabled = fText.enabled;
            if (fText.enabled){
                min = mini_gland.GetComponent<mini_gland_properties>().min_vals[display_state+1];
                max = mini_gland.GetComponent<mini_gland_properties>().max_vals[display_state+1];
                if (display_state == 4){
                    max = (float)(-Math.Log10(min/1000)); // *** HARD CODED *** change H to pH
                    min = (float)(-Math.Log10(max/1000));
                }
                var txt = ion_props[display_state] + "(nM)\n";
                txt += max.ToString("#0");
                txt += "\n\n\n\n\n" + (min+(max-min)/2f).ToString("#0");
                txt += "\n\n\n\n\n" + min.ToString("#0");
                fText.text = txt;
            }
            else fText.text = "";
        }
        //change to pH in python
        // display ion concentration span for the current simulation time step
        if (display_state == -1) return;
        min = mini_gland.GetComponent<mini_gland_properties>().min_vals[display_state+1];
        max = mini_gland.GetComponent<mini_gland_properties>().max_vals[display_state+1];
        var ndvars = mini_gland.GetComponent<mini_gland_properties>().ndvars;
        var ndiscs = mini_gland.GetComponent<mini_gland_properties>().ndiscs;
        dyn_data = mini_gland.GetComponent<mini_gland_properties>().dyn_data;
        idx_vars = mini_gland.GetComponent<mini_gland_properties>().disc_idx + display_state;
        var span_min = dyn_data[idx_vars];
        var span_max = span_min;
        for (int i = 0; i < ndiscs; i++){
            var c = dyn_data[idx_vars + (i * ndvars)];
            if (c < span_min) span_min = c;
            if (c > span_max) span_max = c;
        }
        var offset = 280f * (span_min-min) / (max-min);
        fMin_transform.localPosition = fMin_ref.localPosition + new Vector3(0f, offset, 0f);
        offset = 280f * (span_max-min) / (max-min);
        fMax_transform.localPosition = fMin_ref.localPosition + new Vector3(0f, offset, 0f);
    }
}
