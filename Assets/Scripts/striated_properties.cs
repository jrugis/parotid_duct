using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class striated_properties : MonoBehaviour
{
    public int[] idx_data; 

    // Start is called before the first frame update
    void Start()
    {
        // Cell 22, 34, 46, 51, 54, 55, 56, 63, 66, 74 have no apical faces, thus are skipped
        idx_data = new int[] {
            0,1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16,17,18,19,
            20,-1,21,22,23,24,25,26,27,28,29,
            30,31,-1,32,33,34,35,36,37,38,39,
            40,41,42,-1,43,44,45,46,-1,47,48,-1,-1,-1,49,
            50,51,52,53,54,-1,55,56,-1,57,58,59,
            60,61,62,63,-1,64,65,66,67,68,69,
            70,71,72,73,74,75,76,77,78,79,
            80,81,82,83,84,85,86,87,88,89,
            90,91,92,93,94,95,96,97,98,99,
            100
            };
        Renderer[] renders = GetComponentsInChildren<Renderer>();
        //foreach (Renderer rend in renders){
            //Material mat = rend.GetComponent<Renderer>().material;
            //mat.color = new Color(0,1,0,0.1f);
            //mat.color = Color.Lerp(Color.green, Color.red, (c - min) / (max - min));
            //rend.enabled = (display_state != -1);
        //}
        for (int i=0; i<86; i++){
            if (idx_data[i] == -1) continue;
            renders[i].GetComponent<Renderer>().material.color = new Color(1,0,0,0.1f); 
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
