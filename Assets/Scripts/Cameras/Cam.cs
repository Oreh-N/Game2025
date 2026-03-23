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
		_cam = GetComponent<Camera>();
		height = _cam.orthographicSize * 2;
		width = height * _cam.aspect;
	}


	// Unity uses LEFT-handed coordination system
	// With help of AI
	public Vector3 GetCamProjectionCenter()
	{
		if (_cam == null) return new Vector3();

		Vector3 cam_pos = _cam.transform.position;
		Vector3 f_dir = _cam.transform.forward;
		Vector3 center = cam_pos + f_dir * (-cam_pos.y / f_dir.y);
		center.y = 0;
		return center;
	}

	// With help of AI
	/// <summary>
	/// Computes camera view border points. Precisely, returns approximately (depends on resolution) 
	/// central points of each border side.
	/// </summary>
	/// <returns></returns>
	public List<Vector3> GetCamProjBorderPoints()
	{
		if (_cam == null) return new List<Vector3>();

		var center = GetCamProjectionCenter();
		Vector3 f = _cam.transform.forward;

		float halfH = _cam.orthographicSize / Mathf.Abs(f.y);
		float halfW = width / 2;

		Vector3 forward_dir = Vector3.ProjectOnPlane(f, Vector3.up).normalized;
		Vector3 right_dir = Vector3.ProjectOnPlane(_cam.transform.right, Vector3.up).normalized;

		var top = center + forward_dir * halfH;
		var bottom = center - forward_dir * halfH;
		var right = center + right_dir * halfW;
		var left = center - right_dir * halfW;

		return new List<Vector3>() { top, bottom, right, left };
	}
}

