/*
Attach this to help text object.
*/
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class toggle_text: MonoBehaviour
{
    public KeyCode kcode;
    void Start()
    {
    }
    void Update ()
    {
        if (Input.GetKeyDown (kcode))
        {
            var text = GetComponent<Text>();
            text.enabled = !text.enabled;
        }      
    }  
}