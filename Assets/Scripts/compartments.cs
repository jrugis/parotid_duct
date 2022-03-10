using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class compartments : MonoBehaviour
{
    private ParticleSystem psystem;
    private ParticleSystem.Particle[] points;

    public float deltaTime = 0.1f; // data update period
    private float targetTime;      // next data update time target
    private int tstep;             // current data time step
    private int tsteps;            // total number of data time steps
    private float[,] flow_data;    // flow data [disc, time step] 
    private int ndiscs;            // number of duct discs
    private Vector3[] disc_centers;// disc center coordinates
    private float[] disc_radii;    // disc radii
    private Color[] lut = new Color[512]; // lookup table for flow rate colors
    // create the flow rate color lookup table
    // NOTE: minimum flow is green, maximum flow is yellow,
    //       with nonlinear gradient between
    private void create_lut()
    {
        for (int i = 0; i < lut.Length; i++)
        {
            var c1 = (float)Math.Pow((((float)i) / lut.Length), 0.3f);
            var c2 = (float)Math.Pow((((float)i) / lut.Length), 0.5f);
            lut[i].r = c1;
            lut[i].g = 0.5f + c2;
            lut[i].b = 0.2f - c2;
            lut[i].a = 1f;
            if (lut[i].r > 1f) lut[i].r = 1f; // clamp
            if (lut[i].g > 1f) lut[i].g = 1f;
            if (lut[i].b > 1f) lut[i].b = 1f;
            if (lut[i].r < 0f) lut[i].r = 0f; // clamp
            if (lut[i].g < 0f) lut[i].g = 0f;
            if (lut[i].b < 0f) lut[i].b = 0f;
        }
    }

    // get an int32 count from a file stream
    private Int32 get_count(FileStream fs)
    {
        int bytes = 4;
        var byte_array = new byte[bytes];
        var int32_array = new Int32[1];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, int32_array, 0, bytes);
        return (int32_array[0]);
    }

    // get disc data from a file stream
    private Vector3[] get_disc_data(FileStream fs, int count)
    {
        Vector3[] disc_points = new Vector3[count];
        int bytes = 12;                  // for 3 floats
        var byte_array = new byte[bytes];
        var float_array = new float[3];  // for 1 coordinate
        for (int i = 0; i < count; i++)
        {
            fs.Read(byte_array, 0, bytes); // get a center point
            Buffer.BlockCopy(byte_array, 0, float_array, 0, bytes);
            disc_points[i] = new Vector3(-float_array[0], float_array[2], -float_array[1]);
        }
        return (disc_points);
    }
    // get the flow data from a file stream
    private float[,] get_flow_data(FileStream fs, int count, int steps)
    {
        int bytes = 4 * count * steps;
        var byte_array = new byte[bytes];
        var float_array = new float[count, steps];
        fs.Read(byte_array, 0, bytes);
        Buffer.BlockCopy(byte_array, 0, float_array, 0, bytes);
        return (float_array);
    }
    /*
    // move the position of all particles using flow data velocity
    // NOTE: actually loops the position within point separation distance
    //       as a result, the point ordering within a segment remains the same
    private void move_particle_pos()
    {
        for (int seg = 0; seg < nlsegs; seg++) // segments
        {
            // for visualzation, offset is scaled so that point motion covers less than
            //    half the point separation distance
            var offset = velocity[seg] * pdist * 0.2f * direction[seg];   // change in position

            var p0 = points[points.Length - (nlsegs + 1) + seg].position; // segment start
            var p1 = points[point_idx[seg, 0]].position;                  // current position
            var p2 = p1 + scale * offset;                                 // new position
            var dist = (p2 - p0).magnitude;                               // distance from start
            var pcolor = lut[(int)((lut.Length - 1) * velocity[seg])];

            // past point separation distance?
            if (dist > (scale * pdist)) 
            {
                offset -= pdist * direction[seg]; // jump back by separation distance
            }

            // move interior points
            for (int p = point_idx[seg, 0]; p < point_idx[seg, 1] + 1; p++) 
            {
                points[p].position += scale * offset;
                points[p].startColor = pcolor;
            }

            // hide fractional position "overrun" for last point in segment 
            if (dist > (scale * (length[seg] % pdist)))        // past end of segment?
            {
                pcolor.a = 0f;                            // hide using alpha channel
                points[point_idx[seg, 1]].startColor = pcolor;
            }
        }
    }
    */
    /*
    // set the flow of all discs using flow data
    // NOTE: the data is assumed to be normalized to range 0.0 - 1.0
    private void set_flow(int tstep)
    {
        for (int d = 0; d < ndiscs; d++) // segment
        {
            flow[d] = flow_data[d, tstep];
        }
    }
    */
    private void make_particles(string path)
    {
        // set some particle system parameters
        psystem = gameObject.GetComponent<ParticleSystem>();
        var main = psystem.main;
        main.loop = false;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy; // follow the parent
        main.playOnAwake = false;

        // get particle data
        FileStream fs = new FileStream(path, FileMode.Open);
        ndiscs = get_count(fs);
        Debug.Log("duct discs: " + ndiscs.ToString());
        disc_centers = get_disc_data(fs, ndiscs);
        tsteps = get_count(fs);
        Debug.Log("duct time steps: " + tsteps.ToString());
        get_flow_data(fs, ndiscs, tsteps);
        fs.Close();

        // create particle system points
        points = new ParticleSystem.Particle[ndiscs];
        
        // set particle (disc) positions
        for (int d = 0; d < ndiscs; d++) // discs
        {
            points[d].position = disc_centers[d];
            points[d].startColor = new Color(0f, 1f, 0f);
            points[d].startSize = 1.0f;
        }
    }

    // Use this for initialization
    void Start()
    {
        string path = "_4Unity_duct.bin";
        if (File.Exists(path)) make_particles(path);
        else Debug.Log(path + " not found.");
        tstep = 0;
        //set_velocity(tstep);
        create_lut();
        targetTime = 0.5f; // short startup delay
    }
    // Update is called once per frame
    void Update()
    {
        if (points == null) return;

        var mini_gland = GameObject.Find("MiniGland");
        float currentTime = Time.fixedTime;
        if (currentTime > targetTime)
        {
            int steps = 1 + (int)((currentTime - targetTime) / deltaTime);
            if (steps > 1) Debug.Log("Virtual Duct: frame rate too slow");
            targetTime += deltaTime * steps;
            if (mini_gland.GetComponent<toggle_sim>().simulate) tstep += steps;
            if (tstep >= tsteps) tstep -= tsteps;
            //set_flow(tstep);
        }
        psystem.SetParticles(points, points.Length); // renders the particles
        //if (mini_gland.GetComponent<toggle_sim>().simulate) move_particle_pos();
    }
}
