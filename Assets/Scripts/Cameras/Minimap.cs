using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Minimap : MonoBehaviour
{
    private float width;
    private float height;
    GameObject minimap;

    void Start()
    {
        minimap = GameObject.FindWithTag("Minimap");
		Resolution res = Screen.currentResolution;
        width = Screen.width/5000;
        height = Screen.height/10000;
        Camera camera = minimap.GetComponent<Camera>();
	}

    void Update()
    {
        
    }
}
