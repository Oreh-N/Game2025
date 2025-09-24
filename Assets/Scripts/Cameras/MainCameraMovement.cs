using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraMovement : MonoBehaviour
{
	float _speed = 0.5f; // 1 = 100 % (full speed)
	Vector3 _initPos;
    

    void Start()
    {
		_initPos = transform.position;
	}

	void Update()
    {
		float act_speed = Mathf.Sqrt(_speed);    // actual speed on each axis

		if (Input.GetKey(KeyCode.A))
		{
			if (IsOutOfMap(transform.position + Vector3.left * act_speed) && IsOutOfMap(transform.position + Vector3.forward * act_speed)) return;
			transform.position += Vector3.left * act_speed;
			transform.position += Vector3.forward * act_speed;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			if (IsOutOfMap(transform.position + Vector3.back * act_speed) && IsOutOfMap(transform.position + Vector3.left * act_speed)) return;
			transform.position += Vector3.back * act_speed;
			transform.position += Vector3.left * act_speed;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			if (IsOutOfMap(transform.position + Vector3.right * act_speed) && IsOutOfMap(transform.position + Vector3.back * act_speed)) return;
			transform.position += Vector3.right * act_speed;
			transform.position += Vector3.back * act_speed;
		}
		else if (Input.GetKey(KeyCode.W))
		{
			if (IsOutOfMap(transform.position + Vector3.forward * act_speed) && IsOutOfMap(transform.position + Vector3.right * act_speed)) return;
			transform.position += Vector3.forward * act_speed;
			transform.position += Vector3.right * act_speed;
		}
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		var cam = GetComponent<Camera>();
		float camSideLength = Mathf.Cos(transform.rotation.x) * cam.farClipPlane /4;
		if (pos.x < _initPos.x - 50		 || pos.z < _initPos.z - 50		|| 
			pos.x > 1000 - camSideLength || pos.z > 1000 - camSideLength)
			return true;
		return false;
	}
}
