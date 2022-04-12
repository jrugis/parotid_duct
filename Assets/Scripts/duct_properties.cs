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
    public ions(string name, float min, float max){
        this.name = name;
        this.min = min;
        this.max = max;
    }
}

public class duct_properties : MonoBehaviour
{
    public Text fText;  // for quick reference
    public List<ions> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    void Start()
    {
        ion_props = new List<ions>(){
            new ions("Na", 10.0f, 60.0f),  
            new ions("K", 45.0f, 70.0f),  
            new ions("Cl", 45.0f, 90.0f),  
            new ions("HCO3", 10.0f, 25.0f),  
            new ions("pH", 7.3f, 8.4f)
        };
        fText = GameObject.Find("fluid_display").GetComponent<Text>();
    }
    void Update()
    {
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1;
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renders) rend.enabled = (display_state != -1);
            fText.enabled = (display_state != -1);
        }
        var idx = display_state;
        if (idx == -1) idx = 0;  // index -1 is for "not visible" state so don't use it here!
        fText.text = "Duct Ion Concentration - " + ion_props[idx].name;
    }
}
