﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class acinus : MonoBehaviour
{
    private ParticleSystem psystem;
    private ParticleSystem.Particle[] points;
    public String label;

    void Start()   
    {
        // get terse references to relevant data from MiniGland object
        var mini_gland = GameObject.Find("MiniGland");
        var ntsteps = mini_gland.GetComponent<mini_gland_properties>().a_ntsteps; // number of timesteps
        var nnodes = mini_gland.GetComponent<mini_gland_properties>().a_nnodes;  // number of nodes
        var nodes = mini_gland.GetComponent<mini_gland_properties>().a_nodes;    // node coordinates

        // create the acinus node particles
        psystem = GetComponent<ParticleSystem>();
        var main = psystem.main;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy; // follow the acinus
        points = new ParticleSystem.Particle[nnodes];
        for (int i = 0; i < nnodes; i++)
        {
            points[i].position = nodes[i];
            points[i].startSize = 1.0f;
            //points[i].startColor = new Color(0.9f, 0.1f, 0f);
        }
        psystem.SetParticles(points, points.Length); // renders the nodes

        var psrend = psystem.GetComponent<ParticleSystemRenderer>();
        label = psrend.name;
    }
}
