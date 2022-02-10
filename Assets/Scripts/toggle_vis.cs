/*
Parotid Simulation 
Attach this to a cells prefab object.
*/
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class toggle_vis : MonoBehaviour
{
    public enum States {opaque, transparent, hidden, number_of_states};
    public States state;
    Dictionary<char, KeyCode> chartoKeycode = new Dictionary<char, KeyCode>()
    {
        {'d', KeyCode.D},        // duct inner
        {'i', KeyCode.I},        // intercalated cells
        {'s', KeyCode.S},        // striated cells
        {'1', KeyCode.Alpha1},   // acinus cells
        {'2', KeyCode.Alpha2},   // ...
        {'3', KeyCode.Alpha3},
        {'4', KeyCode.Alpha4},
        {'5', KeyCode.Alpha5},
        {'6', KeyCode.Alpha6}
    };
    void Start()
    {
        state = 0;
    }
    void Update ()
    {
        Color color;
        KeyCode kcode;
        char c = this.name[0];            // get the first letter of the object's name,
        if (c == 'a') c = this.name[1];   //   but for acinii, get the second letter  
        if (chartoKeycode.TryGetValue(c, out kcode))
        {
            if (Input.GetKeyDown (kcode))
            {
                if (++state >= States.number_of_states) state = 0;
                Renderer[] renders = GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renders)
                {
                    if(state == States.opaque)
                    {
                        rend.enabled = true;
                        rend.material.SetOverrideTag("RenderType", "");
                        //rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        rend.material.SetInt("_ZWrite", 1);
                        //rend.material.DisableKeyword("_ALPHATEST_ON");
                        //rend.material.DisableKeyword("_ALPHABLEND_ON");
                        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        rend.material.renderQueue = -1;
                        color = rend.material.GetColor("_Color");;
                        color.a = 1.0f;
                        rend.material.SetColor("_Color", color);
                    }
                    else if(state == States.transparent)
                    {
                        rend.material.SetOverrideTag("RenderType", "Transparent");
                        //rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        rend.material.SetInt("_ZWrite", 0);
                        //rend.material.DisableKeyword("_ALPHATEST_ON");
                        //rend.material.DisableKeyword("_ALPHABLEND_ON");
                        rend.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        rend.material.renderQueue = 3000;
                        color = rend.material.GetColor("_Color");;
                        color.a = 0.0f;
                        rend.material.SetColor("_Color", color);
                    }
                    else if(state == States.hidden)
                    {
                        rend.enabled = false;
                    }
                }
            }
        }
    }
}