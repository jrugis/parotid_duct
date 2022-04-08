using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<ions> ion_props;
    public int display_state;
    public KeyCode kcode; // set this in inspector GUI
    void Start()
    {
        ion_props = new List<ions>(){
            new ions("Na", 10.0f, 60.0f),  
            new ions("K", 0.0f, 100.0f),  
            new ions("Cl", 0.0f, 100.0f),  
            new ions("HCO", 0.0f, 100.0f),  
            new ions("H", 0.0f, 100.0f),  
            new ions("CO2", 0.0f, 100.0f)
        };
    }
    void Update()
    {
        if (Input.GetKeyDown (kcode)){
            if (++display_state >= ion_props.Count) display_state = -1;
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renders) rend.enabled = (display_state != -1);
        }
    }
}
