using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;


public class cycle_vis : MonoBehaviour
{
    public KeyCode kcode; // set this in each object's GUI
    public enum States {opaque, half, transparent, hidden, number_of_states};
    public States state;

    void Update ()
    {
    Color color;
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
                else if(state == States.half)
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
                    color.a = 0.3f;
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