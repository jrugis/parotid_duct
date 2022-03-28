using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class mini_gland_properties : MonoBehaviour
{
    public string path;  // the simulation data file

    // duct fixed data
    public int ndiscs;               // number of duct discs           
    public Vector3[] disc_centers;   // disc centers
    public float[] disc_diameters;   // disc diameters
    public float[] disc_lengths;     // disc lengths
    public Vector3[] disc_dirs;      // disc direction vectors

    // simulation time stepping data
    public int tsteps;            // total number of data time steps
    public int tstep;             // current time step
    public float[] sTimes;        // simulation time at each step
    public float simTime;         // current simulation time 
    private Text tText;           // simulation time display

    // duct dynamic data
    private FileStream fs;
    private long data_head;       // data head file position
    public int nvals;             // number simulated values
    public float[] min_vals;      // minimum data values    
    public float[] max_vals;      // maximum data values    
    public float[] dyn_data;      // simulation values in the current time step
    private Text fText;           // flow rate display

    // data file access functions
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
    
    // Note: Awake functions are executed before any Start functions
     void Awake()
     {
        if (!File.Exists(path))
        {
            Debug.Log("Data file " + path + " not found.");
            Application.Quit();
        }

        // read in duct fixed data
        fs = new FileStream(path, FileMode.Open);
        ndiscs = get_count(fs);                      // number of duct discs           
        disc_centers = get_coordinate(fs, ndiscs);   // disc centers
        disc_diameters = get_floats(fs, ndiscs);     // disc diameters
        disc_lengths = get_floats(fs, ndiscs);       // disc lengths
        disc_dirs = get_coordinate(fs, ndiscs);      // disc direction vectors

        // read in simulation data
        tsteps = get_count(fs);              // time steps
        sTimes = get_floats(fs, tsteps);     // simulation times
        nvals = get_count(fs);               // simulated values
        //min_vals = get_floats(fs, nvals);  // min vals
        //max_vals = get_floats(fs, nvals);  // max vals

        // read initial dynamic data
        data_head = fs.Position;
        dyn_data = get_floats(fs, nvals);
        tstep = 0;
        simTime = sTimes[0];

        // get display components 
        tText = GameObject.Find("time_display").GetComponent<Text>();
        fText = GameObject.Find("flow_display").GetComponent<Text>();
        fText.text = "fluid flow: " + string.Format("{0,4:####}", dyn_data[0]) + " um3/s";
     }
   
    // simulation time stepping
     void Update()
    {
        if (this.GetComponent<toggle_sim>().simulate)
        {
            simTime += Time.deltaTime;
            while (simTime >= sTimes[tstep]){
                dyn_data = get_floats(fs, nvals);
                fText.text = "fluid flow: " + string.Format("{0,4:####}", dyn_data[0]) + " um3/s";
                tstep++;
                if (tstep >= tsteps){    // loop the simulation display
                    tstep = 0;
                    simTime = sTimes[0]; 
                    fs.Seek(data_head, SeekOrigin.Begin);  // rewind the file pointer
                } 
            };
            var sec = simTime % 60;
            var min = Math.Floor(simTime / 60);
            tText.text = " " + min.ToString("0#") + ":" + sec.ToString("0#.00") + "\nmm:ss.ss";
        } 
    }
}
