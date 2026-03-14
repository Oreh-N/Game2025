using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraMovement : MonoBehaviour
{
	float _speed = 3f;
	Vector3 _dir = new Vector3();
	public Vector3 _last_dir = new Vector3();


	void Update()
	{
		_dir = UpdateDir();
		if (_dir != Vector3.zero) _last_dir = _dir;
		if (_dir != Vector3.zero)
		{ MakeStep(_dir); }
	}

	public Vector3 GetDir() { return _dir; }
	public Vector3 GetPos() { return transform.position; }

	private Vector3 UpdateDir()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{ return Vector3.Normalize(Vector3.left + Vector3.forward); }
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{ return Vector3.Normalize(Vector3.back + Vector3.left); }
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{ return Vector3.Normalize(Vector3.right + Vector3.back); }
		else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{ return Vector3.Normalize(Vector3.forward + Vector3.right); }
		return Vector3.zero;
	}

	private void MakeStep(Vector3 dir)
	{
		if (IsOutOfMap(transform.position + dir * _speed))
			return;
		transform.position += dir * _speed;
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		Vector2Int map_lim = new Vector2Int(-20, 1000);
		var cam = GetComponent<Camera>();
		float camSideLength = Mathf.Cos(transform.rotation.x) * cam.farClipPlane / 4;
		if (pos.z < map_lim.x || pos.x > map_lim.y - camSideLength ||
			pos.x < map_lim.x || pos.z > map_lim.y - camSideLength)
			return true;
		return false;
	}

}
