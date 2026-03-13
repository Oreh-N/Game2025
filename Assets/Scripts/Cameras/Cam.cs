using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class Cam : MonoBehaviour
{
	public float width { get; private set; }
	public float height { get; private set; }
	Camera _cam;

	private void Start()
	{
		var cam = GetComponent<Camera>();
		height = cam.orthographicSize * 2;
		width = height * cam.aspect;
		Debug.Log($"W: {width}		H: {height}");
	}


	// Unity uses LEFT-handed coordination system
	public Vector3 FindCamProjectionCenter()
	{
		if (_cam == null) return new Vector3();

		Vector3 c_pos = _cam.transform.position;
		Vector3 f_dir = _cam.transform.forward;
		Vector3 center = c_pos + f_dir * (-c_pos.y / f_dir.y);
		center.y = 0;
		return center;
	}

	public List<Vector3> GetCamProjBorderPoints(Map map)
	{
		if (_cam == null) return new List<Vector3>();

		var center = FindCamProjectionCenter();
		Vector3 forward_dir = Vector3.ProjectOnPlane(_cam.transform.forward, Vector3.up).normalized;
		Vector3 right_dir = Vector3.ProjectOnPlane(_cam.transform.right, Vector3.up).normalized;

		float halfH = height / 2;
		float halfW = width / 2;
		var top = center + forward_dir * halfH;
		var bottom = center - forward_dir * halfH;
		var right = center + right_dir * halfW;
		var left = center - right_dir * halfW;

		return new List<Vector3>() { top, bottom, right, left };
	}
}

