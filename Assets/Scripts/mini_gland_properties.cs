﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class mini_gland_properties : MonoBehaviour
{
    public string path;  // the simulation data file
    public int ndiscs;               // number of duct discs           
    public Vector3[] disc_centers;   // disc centers
    public float[] disc_diameters;   // disc diameters
    public float[] disc_lengths;     // disc lengths
    public Vector3[] disc_dirs;      // disc direction vectors
    private int tsteps;              // simulation data time steps
    public float[] flow_rates;       // simulation flow rate per disc
 
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
    private float[,] get_float_array(FileStream fs, int rows, int cols)
    {
        int bytes = 4 * rows * cols;
        var byte_array = new byte[bytes];
        var float_array = new float[rows, cols];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, float_array, 0, bytes);
        return(float_array);
    }

     void Awake()
     {
        if (!File.Exists(path))
        {
            Debug.Log(path + " not found.");
            Application.Quit();
        }

        // read in virtual duct structural data
        FileStream fs = new FileStream(path, FileMode.Open);
        ndiscs = get_count(fs);                      // number of duct discs           
        Debug.Log("duct discs: " + ndiscs.ToString());
        disc_centers = get_coordinate(fs, ndiscs);   // disc centers
        disc_diameters = get_floats(fs, ndiscs);     // disc diameters
        disc_lengths = get_floats(fs, ndiscs);       // disc lengths
        disc_dirs = get_coordinate(fs, ndiscs);      // disc direction vectors

        // get the simulation data
        tsteps = get_count(fs); // simulation data time steps
        tsteps = 5000; // *********** TEMPORARY ************
        Debug.Log("duct time steps: " + tsteps.ToString());
        flow_rates = get_floats(fs, ndiscs);
        fs.Close();
     }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}