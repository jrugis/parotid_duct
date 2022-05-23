using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;


public class toggle_cutaway : MonoBehaviour
{
    public KeyCode kcode; // set this in each object's GUI
    public bool cutaway;
    public List<int> cells;

    void Update ()
    {
        if (Input.GetKeyDown (kcode))
        {
            cutaway = !cutaway;;
            foreach (var c in cells){
                this.gameObject.transform.GetChild(c-1).gameObject.SetActive(!cutaway);
            }
        }
    }
}
