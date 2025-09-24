using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraBorder : MonoBehaviour
{
    LineRenderer _camBorderLine;


	void Start()
    {
        _camBorderLine = GetComponentInChildren<LineRenderer>();
        UpdateProjection();
    }

	void UpdateProjection()
	{
        float display_ratio = Screen.width / Screen.height;
		Vector3[] line_pos = { new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3() };
		float cam_x_angle = Camera.main.transform.rotation.x;
		float angle_with_ground = 90 - cam_x_angle;
		float ground_catheti = Camera.main.transform.position.y * Mathf.Tan(cam_x_angle);
		float projection_width = Camera.main.orthographicSize * display_ratio;
		float projection_length = Camera.main.orthographicSize / Mathf.Sin(angle_with_ground);
		float projection_height = Camera.main.transform.position.y * 2;
		line_pos[0].x = projection_height;
		line_pos[1] = new Vector3(projection_height, projection_width, projection_width);
		line_pos[2] = new Vector3(-projection_height, projection_width, projection_width);
		line_pos[3].x = -projection_height;
		line_pos[4].x = line_pos[0].x;

		_camBorderLine.SetPositions(line_pos);
	}
}
/*
 
 
 * (Camera)
 |\
 |a\
 |  \
 |   \
 |    \
 |____b\_______________ ground
   ^
   |
   |
(ground catheti) 


a,b - angles

 */
