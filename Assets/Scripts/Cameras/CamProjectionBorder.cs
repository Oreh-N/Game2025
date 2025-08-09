using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraBorder : MonoBehaviour
{
    Camera cam;
    float cam_x_angle;
    float angle_with_ground;
    float ground_catheti;
    float projection_width;
    float projection_length;
    float projection_height;
    LineRenderer line;
    Vector3[] line_pos = { new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3() };
    float display_ratio;


	void Start()
    {
        cam = Camera.main;
        line = GetComponentInChildren<LineRenderer>();
        display_ratio = Screen.width / Screen.height;
    }

    void Update()
    {
        cam_x_angle = cam.transform.rotation.x;
        angle_with_ground = 90 - cam_x_angle;
        ground_catheti = cam.transform.position.y * Mathf.Tan(cam_x_angle);
        projection_width = cam.orthographicSize * display_ratio;
        projection_length = cam.orthographicSize/ Mathf.Sin(angle_with_ground);
        projection_height = cam.transform.position.y * 2;
        line_pos[0].x = projection_width / 2;
        line_pos[1] = new Vector3(projection_width / 2 , projection_height, projection_height);
		line_pos[2] = new Vector3(-projection_width / 2, projection_height, projection_height);
		line_pos[3].x = -projection_width / 2;
		line_pos[4].x = line_pos[0].x;

		line.SetPositions(line_pos);
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
