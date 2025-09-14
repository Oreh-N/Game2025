using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class BuildingManager : MonoBehaviour
{
	public LayerMask ground;
	 GameObject building;
	 Camera cam;
	 bool allowBuilding = false;


	private void Start()
	{
		cam = Camera.main;


	}

	void Update()
	{
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

		if (Input.GetMouseButtonDown(1) && allowBuilding)
		{
			ray = cam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a clickble object
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
			{
				Vector3 newObjPos = new Vector3(hit.point.x, building.transform.position.y, hit.point.z);
				Instantiate(building, newObjPos, Quaternion.identity);
				Debug.Log(newObjPos);
				allowBuilding = false;
			}
		}
	}

	

	public void AllowBuilding(GameObject spawnBuilding)
	{
		allowBuilding = true;
		building = spawnBuilding;
		Debug.Log(allowBuilding);
	}



}
