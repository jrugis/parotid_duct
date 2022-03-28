using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_time : MonoBehaviour
{
    public float seconds = 0;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey (KeyCode.LeftShift)) return;  
        if (Input.GetMouseButton(0))
        {
            seconds += Input.GetAxis("Mouse X");
        }
    }
}
