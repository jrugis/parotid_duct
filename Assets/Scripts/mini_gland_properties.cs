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
    public int ndiscs;           // number of duct discs
    public int ncells;           // number of duct cells
    public int ndvars;           // number disc variables 
    public int ncvars;           // number of cell variables
    public int disc_idx;
    public Vector3[] disc_centers;   // disc centers
    public float[] disc_diameters;   // disc diameters
    public float[] disc_lengths;     // disc lengths
    public Vector3[] disc_dirs;      // disc direction vectors

    // simulation time stepping data
    public int tsteps;            // total number of data time steps
    public int tstep;             // current time step
    private int prev_tstep;       // current time step
    public float[] sTimes;        // simulation time at each step
    private int stim_on;          // stimulation ON time step
    private int stim_off;         //       "     OFF   "   "
    public float simTime;         // current simulation time 
    private Text tText;           // simulation time display
    public float speed;           // playback speed
    private Text sText;           // playback speed display

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
    
    private void display_duct_data()
    {
        string stim = "OFF";
        if ((tstep >= stim_on) & (tstep < stim_off)) stim = "ON";
        string t = "\n" + dyn_data[disc_idx].ToString("G3");
        t += "\n" + dyn_data[disc_idx+1].ToString("G3");
        t += "\n" + dyn_data[disc_idx+2].ToString("G3");
        t += "\n" + dyn_data[disc_idx+3].ToString("G3");
        t += "\n" + (-Math.Log10(dyn_data[disc_idx+4]/1000)).ToString("G3");
        t += "\n" + dyn_data[0].ToString("0#");
        t += "\n\n" + stim;
        fText.text = t;
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
        stim_on = get_count(fs);             // stimulation ON time step 
        stim_off = get_count(fs);            // stimulation OFF time step
        sTimes = get_floats(fs, tsteps);     // simulation times
        nvals = get_count(fs);               // simulated values
        //min_vals = get_floats(fs, nvals);  // min vals
        //max_vals = get_floats(fs, nvals);  // max vals

        // read initial dynamic data
        data_head = fs.Position;
        dyn_data = get_floats(fs, nvals);
        simTime = sTimes[0];
        disc_idx = ndiscs + (ncvars * ncells);   // index to the disc data

        // get display components 
        tText = GameObject.Find("time_display").GetComponent<Text>();
        fText = GameObject.Find("duct_display").GetComponent<Text>();
        sText = GameObject.Find("speed_display").GetComponent<Text>();
        prev_tstep = -1;  // to force initial data display  
    }
   
    // simulation time stepping
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.P)){
            speed *= 2F;
            if (speed > 16) speed = 1.0F / 8;
            sText.text = "speed " + speed.ToString() + "x";
        }
        if (Input.GetKey (KeyCode.LeftShift) & Input.GetMouseButton(0)){
            simTime += ((sTimes[tsteps-1] - sTimes[0]) / 30) * Input.GetAxis("Mouse X");
            if (simTime < sTimes[0]) simTime = sTimes[0];
            if (simTime > sTimes[tsteps-1]) simTime = sTimes[tsteps-1];
            tstep = 0;
        }
        if (this.GetComponent<toggle_sim>().simulate){
            simTime += speed * Time.deltaTime;
        } 
        while (simTime > sTimes[tstep]){
            tstep++;
            if (tstep >= tsteps){    // back to the beginning
                tstep = 0;
                simTime = sTimes[0]; 
            } 
        }
        if (tstep != prev_tstep){
            prev_tstep = tstep;
            fs.Seek(data_head + (tstep * nvals * sizeof(float)), SeekOrigin.Begin);  // position in file
            dyn_data = get_floats(fs, nvals);                                        // get the data
            display_duct_data();                   // display the dynamic duct data
            var sec = simTime % 60;                // display the simulation time
            var min = Math.Floor(simTime / 60);    //
            tText.text = " " + min.ToString("0#") + ":" + sec.ToString("0#.00") + "\nmm:ss.ss";
        }
    }
}
