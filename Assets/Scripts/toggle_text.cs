/*
Attach this to help text object.
*/
using System;
using System.Text;
using TMPro;
using UnityEngine;


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
            GetComponent<TMPro.TextMeshProUGUI>().enabled = !GetComponent<TMPro.TextMeshProUGUI>().enabled;
        }      
    }  
}