using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Text fText;          // for quick reference
    public RawImage fColorBar;  // 
    public RawImage fLmarker;   // 
    public RawImage fRmarker;   // 
    public List<string> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
        fText = GameObject.Find("fluid_display").GetComponent<Text>();
        fColorBar = GameObject.Find("color_bar").GetComponent<RawImage>();
        fLmarker = GameObject.Find("Lmarker").GetComponent<RawImage>();
        fRmarker = GameObject.Find("Rmarker").GetComponent<RawImage>();
        ion_props = new List<string>(){"Na", "K", "Cl", "HCO3", "pH"};
    }
    void Update()
    {
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1;
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renders) rend.enabled = (display_state != -1);
            fText.enabled = (display_state != -1);  // index -1 is for "not visible" state
            fColorBar.enabled = fLmarker.enabled = fRmarker.enabled = fText.enabled;
            if (fText.enabled){
                var min = mini_gland.GetComponent<mini_gland_properties>().min_vals[display_state+1];
                var max = mini_gland.GetComponent<mini_gland_properties>().max_vals[display_state+1];
                if (display_state == 4){
                    max = (float)(-Math.Log10(min/1000)); // *** HARD CODED *** change H to pH
                    min = (float)(-Math.Log10(max/1000));
                }
                var txt = "Duct Ion Concentration\n                           " + ion_props[display_state];
                txt += "       " + min.ToString("#0.0");
                txt += "                         " + max.ToString("#0.0");
                fText.text = txt;
            }
            else fText.text = "";
        }
        //change to pH in python
        //int[] numbers = { 5, 36, 23, 45, 15, 92, -5, 3, 33 };
        //int min = numbers.Min();
    }
}
