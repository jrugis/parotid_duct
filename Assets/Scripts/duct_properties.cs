using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class duct_properties : MonoBehaviour
{
    private GameObject mini_gland;
    private Transform fMin_ref;
    public Text fBG;  // 
    public Text fText;          // for quick reference
    public RawImage fColorBar;  // 
    public RawImage fMin_image;
    public Text fMin_val;
    public Transform fMin_transform;
    public RawImage fMax_image;
    public Text fMax_val;
    public Transform fMax_transform;
    public List<string> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    public float[] dyn_data;
    public float[] c;
    public int idx_vars;
    void Start()
    {
        ion_props = new List<string>(){"Na", "K", "Cl", "HCO3", "pH"};
        mini_gland = GameObject.Find("MiniGland");

        fBG = GameObject.Find("fluid_bg").GetComponent<Text>();
        fText = GameObject.Find("fluid_display").GetComponent<Text>();
        fColorBar = GameObject.Find("fluid_color_bar").GetComponent<RawImage>();
        fMin_ref = GameObject.Find("min_fluid_ref").GetComponent<Transform>();

        fMin_image = GameObject.Find("min_fluid_marker").GetComponent<RawImage>();
        fMin_transform = GameObject.Find("min_fluid_marker").GetComponent<Transform>();
        fMin_val = GameObject.Find("min_fluid_val").GetComponent<Text>();

        fMax_image = GameObject.Find("max_fluid_marker").GetComponent<RawImage>();
        fMax_transform = GameObject.Find("max_fluid_marker").GetComponent<Transform>();
        fMax_val = GameObject.Find("max_fluid_val").GetComponent<Text>();
    }
    void Update()
    {
        float min, max;
        var ncvars = mini_gland.GetComponent<mini_gland_properties>().ncvars;
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1; // display state wraps

            // change visibility of all duct discs
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renders) rend.enabled = (display_state != -1);

            // change visibility of all fluid display components
            fText.enabled = (display_state != -1);  // index -1 is for "not visible" state
            fBG.enabled = fColorBar.enabled = fText.enabled;
            fMin_image.enabled = fMax_image.enabled = fText.enabled;
            fMin_val.enabled = fMax_val.enabled = fText.enabled;

            // display fluid color bar scale numbers
            if (fText.enabled){
                min = mini_gland.GetComponent<mini_gland_properties>().min_vals[ncvars+display_state];
                max = mini_gland.GetComponent<mini_gland_properties>().max_vals[ncvars+display_state];
                var txt = ion_props[display_state] + (display_state==4 ? "\n" : "(mM)\n");
                txt += (display_state==4 ? max.ToString("#0.0") : max.ToString("#0")); // higher precision for pH
                txt += "\n\n\n\n\n";
                var mid = min+(max-min)/2f;
                txt += (display_state==4 ? mid.ToString("#0.0") : mid.ToString("#0"));
                txt += "\n\n\n\n\n";
                txt += (display_state==4 ? min.ToString("#0.0") : min.ToString("#0"));
                fText.text = txt;
            }
            else fText.text = "";
        }
        if (display_state == -1) return; // done if not visi1ble

        // display ion concentration span for the current simulation time step
        min = mini_gland.GetComponent<mini_gland_properties>().min_vals[ncvars+display_state];
        max = mini_gland.GetComponent<mini_gland_properties>().max_vals[ncvars+display_state];
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
        var offset = 280f * (span_min-min) / (max-min);  // min marker position and value
        fMin_transform.localPosition = fMin_ref.localPosition + new Vector3(0f, offset, 0f);
        fMin_val.text = span_min.ToString("#0.0");

        offset = 280f * (span_max-min) / (max-min);      // max marker position and value    
        fMax_transform.localPosition = fMin_ref.localPosition + new Vector3(0f, offset, 0f);
        fMax_val.text = span_max.ToString("#0.0");
    }
}
