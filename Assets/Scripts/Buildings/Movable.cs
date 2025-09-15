using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	Vector3 shift;

	private void OnMouseDown()
	{
		shift = transform.position - BuildingManager.GetMouseWorldPos();
		Debug.Log("OnMouseDown");
		Debug.Log(shift);
		Debug.Log(BuildingManager.GetMouseWorldPos());
	}

	private void OnMouseDrag()
	{
		Vector3 newPos = BuildingManager.GetMouseWorldPos() + shift;
		transform.position = BuildingManager.Instance.MapCoordToGrid(newPos);

		Debug.Log("OnMouseDrag");
		Debug.Log(newPos);
		Debug.Log(BuildingManager.Instance.MapCoordToGrid(newPos));
	}
}
