using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class discs : MonoBehaviour
{
    public GameObject disc;           // the object to clone
    private GameObject _mini_gland;  // for quick reference to base parent
    public int _tstep = 0;            // current data time step
    private int _tsteps;              // total number of data time steps
    public float _deltaTime = 0.1f;  // data update period
    private float _targetTime = 0.5f; // initial short startup delay
 
    private Int32 get_count(FileStream fs)
    {
        int bytes = 4;   // for 32 bit integer
        var byte_array = new byte[bytes];
        var int32_array = new Int32[1];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, int32_array, 0, bytes);
        return (int32_array[0]);
    }
    private float[] get_floats(FileStream fs, int count)
    {
        int bytes = 4 * count;  // for 32 bit floats
        var byte_array = new byte[bytes];
        var float_array = new float[count];
        fs.Read(byte_array, 0, bytes);     
        Buffer.BlockCopy(byte_array, 0, float_array, 0, bytes);
        return(float_array);
    }
    private Vector3[] get_coordinate(FileStream fs, int count)
    {
        int bytes = 12;                 // for 3 floats (1 coordinate)
        var byte_array = new byte[bytes];
        var coordinate = new float[3];  // for 1 coordinate
        var coordinate_array = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            fs.Read(byte_array, 0, bytes); // get a center point
            Buffer.BlockCopy(byte_array, 0, coordinate, 0, bytes);
            coordinate_array[i] = new Vector3(-coordinate[0], coordinate[2], -coordinate[1]);
        }
        return(coordinate_array);
    }
    private float[,] get_flow_data(FileStream fs, int rows, int cols)
    {
        int bytes = 4 * rows * cols;
        var byte_array = new byte[bytes];
        var flow_data = new float[rows, cols];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, flow_data, 0, bytes);
        return(flow_data);
    }
    private void make_discs(string path)
    {
        // get the disc data
        FileStream fs = new FileStream(path, FileMode.Open);
        var ndiscs = get_count(fs);                      // number of duct discs           
        Debug.Log("duct discs: " + ndiscs.ToString());
        var disc_centers = get_coordinate(fs, ndiscs);   // disc centers
        var disc_diameters = get_floats(fs, ndiscs);     // disc diameters
        var disc_lengths = get_floats(fs, ndiscs);       // disc lengths
        var disc_dirs = get_coordinate(fs, ndiscs);      // disc direction vectors

        // get the simulation data
        _tsteps = get_count(fs); // simulation data time steps
        _tsteps = 5000; // *********** TEMPORARY ************
        Debug.Log("duct time steps: " + _tsteps.ToString());
        var flow_data = get_flow_data(fs, _tsteps, ndiscs);
        fs.Close();

        // create the disc objects
        float scale = 0.01F; 
        for (int d = 0; d < ndiscs; d++)
        {
            GameObject obj = Instantiate(disc);
            obj.GetComponent<disc_properties>()._disc_id = d; // set disc id 
            obj.transform.SetParent(this.transform);          // place in the hierarchy
 
            //set position and orientation
            float s = disc_diameters[d]*scale; 
            obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(s,s,s));
            obj.transform.LookAt(disc_dirs[d]);
            obj.transform.Rotate(90,0,0);
            obj.transform.position += scale * disc_centers[d] + new Vector3(0F,0.30F,0F); // FIX Y-OFFSET ????
 
            // randomly offset the texture x-position so that adjecent textures don't align
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material.mainTextureOffset = new Vector2(UnityEngine.Random.Range(0, Mathf.PI), UnityEngine.Random.Range(0, Mathf.PI));
            rend.enabled = false;  // start hidden

            // copy associated flow velocity to disc object
            float[] flow = new float[_tsteps];                                     // flow velocity
            float area = Mathf.PI * disc_diameters[d] * disc_diameters[d] / 4.0F;  // disc cross-section area
            for (int t = 0; t < _tsteps; t++) flow[t] = flow_data[t,d] / area;     // convert flow rate to velocity
            obj.GetComponent<disc_properties>()._flow_data = flow;
        }
    }

    void Start()
    {
        _mini_gland = GameObject.Find("MiniGland");

        string path = "_4Unity_duct.bin";           // the simulation data file
        if (File.Exists(path)) make_discs(path);    // create the disc clones
        else Debug.Log(path + " not found.");
    }

    void Update()
    {
        float x = _targetTime;
        float y = _deltaTime;
    }
}
