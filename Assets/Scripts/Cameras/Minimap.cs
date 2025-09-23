using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Minimap : MonoBehaviour
{
    GameObject _minimap;
    float _height;
    float _width;

    void Start()
    {
        _minimap = GameObject.FindWithTag("Minimap");
		Resolution res = Screen.currentResolution;
        _width = Screen.width/5000;
        _height = Screen.height/10000;
        Camera camera = _minimap.GetComponent<Camera>();
	}

    void Update()
    {
        
    }
}
