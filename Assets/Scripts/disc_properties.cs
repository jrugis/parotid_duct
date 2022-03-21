using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disc_properties : MonoBehaviour
{
    public int _disc_id; 
    public float[] _flow_data;
    private GameObject _mini_gland;    // for quick reference to base parent
    private GameObject _parent;        // the disc object 
     
    // Start is called before the first frame update
    void Start()
    {
        _mini_gland = GameObject.Find("MiniGland");
        _parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_mini_gland.GetComponent<toggle_sim>().simulate) return;

        // visualise flow velocity
        Material mat = GetComponent<Renderer>().material;
        float x = mat.mainTextureOffset.x;
        float y = mat.mainTextureOffset.y + 10.0F * _flow_data[_parent.GetComponent<discs>()._tstep] * Time.deltaTime;
        mat.mainTextureOffset = new Vector2(x, y);
    }
}
