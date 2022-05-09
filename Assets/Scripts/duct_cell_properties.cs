using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class duct_cell_properties : MonoBehaviour
{
    private GameObject mini_gland;
    public Text cBG;  // 
    public Text cText;          // for quick reference
    public RawImage cColorBar;  // 
    Renderer[] renderers;     // the cell renderers
    public List<string> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    public float min, max;

    // Start is called before the first frame update
    void Start()
    {
        ion_props = new List<string>(){"Na", "K", "Cl", "HCO3", "pH"};
        mini_gland = GameObject.Find("MiniGland");
        cBG = GameObject.Find("duct_cell_bg").GetComponent<Text>();
        cText = GameObject.Find("duct_cell_display").GetComponent<Text>();
        cColorBar = GameObject.Find("duct_cell_color_bar").GetComponent<RawImage>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // cycle to next ion species?
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1; // display state wraps
            cText.enabled = (display_state!= -1);
            cBG.enabled = cColorBar.enabled = cText.enabled;
            if (display_state != -1){
                min = mini_gland.GetComponent<mini_gland_properties>().min_vals[display_state];
                max = mini_gland.GetComponent<mini_gland_properties>().max_vals[display_state];
            }
        }

        // display cell ion concentration 
        var ndiscs = mini_gland.GetComponent<mini_gland_properties>().ndiscs;
        var ncvars = mini_gland.GetComponent<mini_gland_properties>().ncvars;
        foreach (var rend in renderers){  // for each duct cell
            var s = rend.name;  // name of duct cell (which includes its number)
            var idx = Convert.ToInt32((s.Substring(s.Length-3)))-1;  // zero based intger
            Material mat = rend.GetComponent<Renderer>().material;
 
            var a = mat.color.a;   // retain the alpha component of existing color
            if (display_state == -1){
                mat.color = new Color(1,1,1,a);
            }
            else{
                var c = mini_gland.GetComponent<mini_gland_properties>().dyn_data[ndiscs + ncvars*idx + display_state];
                var cl = Color.Lerp(Color.green, Color.red, (c - min) / (max - min));
                cl.a = a;
                mat.color = cl;
            }
        }

    }
}

//var meshes = GetComponentsInChildren<MeshFilter>();
//print(meshes[0].name);
