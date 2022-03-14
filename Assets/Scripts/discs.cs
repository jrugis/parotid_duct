using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class discs : MonoBehaviour
{
   public GameObject disc;

    //private float _deltaTime = 0.1f; // data update period
    //private float _targetTime;      // next data update time target
    //private int _tstep;             // current data time step
    private int _tsteps;              // total number of data time steps
    private float[,] _flow_data;      // flow data [disc, time step] 
 
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
    private void get_flow_data(FileStream fs, int count, int steps)
    {
        int bytes = 4 * count * steps;
        var byte_array = new byte[bytes];
        var _flow_data = new float[count, steps];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, _flow_data, 0, bytes);
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
        Debug.Log("duct time steps: " + _tsteps.ToString());
        get_flow_data(fs, ndiscs, _tsteps);      
        fs.Close();

        // create the disc objects
        float scale = 0.01f; 
        for (int i = 0; i < ndiscs; i++)
        {
            GameObject obj = Instantiate(disc);
            obj.transform.SetParent(this.transform.parent);
            //obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(disc_diameters[i]*scale, 0.9f*disc_lengths[i]*scale, disc_diameters[i]*scale));
            obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(disc_diameters[i]*scale, disc_diameters[i]*scale, disc_diameters[i]*scale));
            obj.transform.LookAt(disc_dirs[i]);
            obj.transform.Rotate(90,0,0);
            obj.transform.position += scale * disc_centers[i] + new Vector3(0f,0.30f,0f);
            /*
            if(i==0)
            {
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                Vector3[] vertices = mesh.vertices;
                Color[] colors = new Color[vertices.Length];
                for (int j = 0; j < vertices.Length; j++)
                    colors[j] = new Color(1f,0f,0f);
                mesh.colors = colors;
            }
            */
        }
    }

    void Start()
    {
        string path = "_4Unity_duct.bin";
        if (File.Exists(path)) make_discs(path);
        else Debug.Log(path + " not found.");
        //tstep = 0;
        //set_velocity(tstep);
        //create_lut();
        //targetTime = 0.5f; // short startup delay
    }
}
/*
    foreach(Transform child in transform)
    {
        Something(child.gameObject);
    }
position, scale, orientation
vertex colors???
cast shadows OFF
recieve shadows OFF
*/