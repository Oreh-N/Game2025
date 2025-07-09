using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject mainCam;
    
    void Start()
    {
		mainCam = GameObject.FindWithTag("MainCamera");
		if (mainCam == null) 
		{ Debug.Log($"No camera found"); }
	}


	void Update()
    {
		TryMove();
    }

	private void TryMove()
	{
		int fact = 1;

		if (Input.GetKeyDown(KeyCode.A))
        { mainCam.transform.position += Vector3.left * fact; }
		else if (Input.GetKeyDown(KeyCode.S))
		{ mainCam.transform.position += Vector3.back * fact; }
		else if (Input.GetKeyDown(KeyCode.D))
		{ mainCam.transform.position += Vector3.right * fact; }
		else if (Input.GetKeyDown(KeyCode.W))
		{ mainCam.transform.position += Vector3.fwd * fact; }
	}
}
