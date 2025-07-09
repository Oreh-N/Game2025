using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera main;
    
    void Start()
    {
        main = GetComponentInChildren<Camera>();
    }

    
    void Update()
    {
        TryMove();
    }

	private void TryMove()
	{
		throw new NotImplementedException();
	}
}
