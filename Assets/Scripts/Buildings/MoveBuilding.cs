using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuilding : MonoBehaviour
{
	Vector3 offset;

	private void OnMouseDown()
	{
		offset = transform.position - BuildingManager.GetMouseWorldPos();
	}

	private void OnMouseDrag()
	{
		Vector3 pos = BuildingManager.GetMouseWorldPos() + offset;
		transform.position = BuildingManager.Instance.MapCoordToGrid(pos);

	}
}
