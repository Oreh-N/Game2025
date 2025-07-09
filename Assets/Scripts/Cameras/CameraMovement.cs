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
		float speed = 0.05f; // 1 = 100 % (full speed)

		if (Input.GetKey(KeyCode.A))
        { mainCam.transform.position += Vector3.left * speed; }
		if (Input.GetKey(KeyCode.S))
		{ mainCam.transform.position += Vector3.back * speed; }
		if (Input.GetKey(KeyCode.D))
		{ mainCam.transform.position += Vector3.right * speed; }
		if (Input.GetKey(KeyCode.W))
		{ mainCam.transform.position += Vector3.forward * speed; }
	}
}
