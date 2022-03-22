using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disc_properties : MonoBehaviour
{
    public int disc_id; 
    public float[] flow_velocity;
    private GameObject mini_gland;    // for quick reference to base parent
    // Start is called before the first frame update
    void Start()
    {
        mini_gland = GameObject.Find("MiniGland");
    }

    void Update()
    {
        if (!mini_gland.GetComponent<toggle_sim>().simulate) return;
        var tstep = mini_gland.GetComponent<mini_gland_properties>().tstep;

        // visualise flow velocity
        Material mat = GetComponent<Renderer>().material;
        float x = mat.mainTextureOffset.x;
        float y = mat.mainTextureOffset.y + (flow_velocity[tstep] / 10.24F); // scaled by texture size
        mat.mainTextureOffset = new Vector2(x, y);
    }
}
