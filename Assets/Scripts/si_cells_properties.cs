using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class si_cells_properties : MonoBehaviour
{
    private GameObject mini_gland;
    private Transform cMin_ref;
    public Text cBG;  // 
    public Text cText;          // for quick reference
    public RawImage cColorBar;  // 
    public RawImage cMin_image;
    public Text cMin_val;
    public Transform cMin_transform;
    public RawImage cMax_image;
    public Text cMax_val;
    public Transform cMax_transform;
    public List<string> ion_props;
    public List<Color> ion_colors;
    public List<string> ion_color_bars;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    public float min, max;

    void Start()
    {
        ion_props = new List<string>(){"Na", "K", "Cl", "HCO3", "pH"};
        ion_colors = new List<Color>(){new Color(0.00f, 0.00f, 0.00f),  // NULL placeholder, for OFF state 
                                       new Color(1.00f, 0.10f, 0.10f),  // first ion color 
                                       new Color(1.00f, 0.37f, 0.20f),  // etc 
                                       new Color(1.00f, 0.85f, 0.10f),  // 
                                       new Color(0.10f, 0.80f, 0.10f),  // 
                                       new Color(0.10f, 0.60f, 1.00f)}; // 
        ion_color_bars = new List<string>(){"red_bar", // first ion color bar
                                            "orange_bar", // etc
                                            "yellow_bar", 
                                            "green_bar", 
                                            "blue_bar"};
        mini_gland = GameObject.Find("MiniGland");

        cBG = GameObject.Find("duct_cell_bg").GetComponent<Text>();
        cText = GameObject.Find("duct_cell_display").GetComponent<Text>();
        cColorBar = GameObject.Find("duct_cell_color_bar").GetComponent<RawImage>();
        cMin_ref = GameObject.Find("min_cell_ref").GetComponent<Transform>();

        cMin_image = GameObject.Find("min_cell_marker").GetComponent<RawImage>();
        cMin_transform = GameObject.Find("min_cell_marker").GetComponent<Transform>();
        cMin_val = GameObject.Find("min_cell_val").GetComponent<Text>();

        cMax_image = GameObject.Find("max_cell_marker").GetComponent<RawImage>();
        cMax_transform = GameObject.Find("max_cell_marker").GetComponent<Transform>();
        cMax_val = GameObject.Find("max_cell_val").GetComponent<Text>();
    }
    void Update()
    {
        // cycle to next ion species?
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1; // display state wraps
            cText.enabled = (display_state!= -1);
            cBG.enabled = cColorBar.enabled = cText.enabled;
            cMin_image.enabled = cMin_val.enabled = cText.enabled;
            cMax_image.enabled = cMax_val.enabled = cText.enabled;
            if (display_state!= -1) cColorBar.texture = Resources.Load<Texture2D>(ion_color_bars[display_state]);
        }

        // display cell concentration color bar scale numbers
        if (cText.enabled & display_state!=-1){
            min = mini_gland.GetComponent<mini_gland_properties>().min_vals[display_state];
            max = mini_gland.GetComponent<mini_gland_properties>().max_vals[display_state];
            var txt = ion_props[display_state] + (display_state==4 ? "\n" : " (mM)\n");
            txt += (display_state==4 ? max.ToString("#0.0") : max.ToString("#0.0")); // higher precision for pH?
            txt += "\n\n\n\n\n";
            var mid = min+(max-min)/2f;
            txt += (display_state==4 ? mid.ToString("#0.0") : mid.ToString("#0.0"));
            txt += "\n\n\n\n\n";
            txt += (display_state==4 ? min.ToString("#0.0") : min.ToString("#0.0"));
            cText.text = txt;
        }
        else cText.text = "";
        if (display_state == -1) return; // done if not visi1ble

        // display ion concentration span for the current simulation time step
        var ncells = mini_gland.GetComponent<mini_gland_properties>().ncells;
        var ncvars = mini_gland.GetComponent<mini_gland_properties>().ncvars;
        var ndvars = mini_gland.GetComponent<mini_gland_properties>().ndvars;
        var ndiscs = mini_gland.GetComponent<mini_gland_properties>().ndiscs;
        var dyn_data = mini_gland.GetComponent<mini_gland_properties>().dyn_data;

        var span_min = dyn_data[display_state];  // the first value, hopefully not a dead cell!
        var span_max = span_min;

        for (int i = 0; i < ncells; i++){
            var c = dyn_data[ncvars*i + display_state];
            if (c == 0) continue;  // skip dead cells
            if (c < span_min) span_min = c;
            if (c > span_max) span_max = c;
        }

        var offset = 100f * (span_min-min) / (max-min);  // min marker position and value
        cMin_transform.localPosition = cMin_ref.localPosition + new Vector3(offset, 0f, 0f);
        cMin_val.text = span_min.ToString("#0.00");

        offset = 100f * (span_max-min) / (max-min);      // max marker position and value    
        cMax_transform.localPosition = cMin_ref.localPosition + new Vector3(offset, 0f, 0f);
        cMax_val.text = span_max.ToString("#0.00");
    }
}
