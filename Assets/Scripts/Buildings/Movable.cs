using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	Vector3 offset;

	private void OnMouseDown()
	{
		offset = transform.position - BuildingManager.GetMouseWorldPos() + BuildingManager.Instance._currBuildHeight;
	}

	private void OnMouseDrag()
	{
		Vector3 pos = BuildingManager.GetMouseWorldPos() + offset;
		transform.position = BuildingManager.Instance.MapCoordToGrid(pos) + BuildingManager.Instance._currBuildHeight;

	}
}
