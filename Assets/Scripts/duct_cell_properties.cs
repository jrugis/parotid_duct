using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class duct_cell_properties : MonoBehaviour
{
    private GameObject mini_gland;
    private GameObject si_cells;
    Renderer[] renderers;     // the cell renderers

    // Start is called before the first frame update
    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
        si_cells = GameObject.Find("si_cells");
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // display cell ion concentration 
        var ndiscs = mini_gland.GetComponent<mini_gland_properties>().ndiscs;
        var ncvars = mini_gland.GetComponent<mini_gland_properties>().ncvars;
        var display_state = si_cells.GetComponent<si_cells_properties>().display_state;
        var min = si_cells.GetComponent<si_cells_properties>().min;
        var max = si_cells.GetComponent<si_cells_properties>().max;
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
                if (c == 0) mat.color = new Color(1,1,1,a);
                else{
                    var cl = Color.Lerp(Color.green, Color.red, (c - min) / (max - min));
                    cl.a = a;
                    mat.color = cl;
                }
            }
        }
    }
}

//var meshes = GetComponentsInChildren<MeshFilter>();
//print(meshes[0].name);
