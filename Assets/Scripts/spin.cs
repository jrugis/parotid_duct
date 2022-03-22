using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class spin: MonoBehaviour
{
    private EventSystem event_system;
    void Start()
    {
        event_system = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }
    void Update ()
    {
        //if (event_system.IsPointerOverGameObject()) return;        // blocks spinning over text
        if (event_system.currentSelectedGameObject != null) return;  // blocks spinning over slider
        if (Input.GetKey (KeyCode.LeftShift)) return;  
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, -5.0F * Input.GetAxis("Mouse X"));
            transform.RotateAround(Vector3.zero, Vector3.right, 5.0F * Input.GetAxis("Mouse Y"));
        }
    }  
}