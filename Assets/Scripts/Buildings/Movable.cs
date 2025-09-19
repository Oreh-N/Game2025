using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	Vector3 shift;

	private void OnMouseDrag()
	{
		transform.position = BuildingManager.Instance.MapCoordToGrid(BuildingManager.GetMouseWorldPos());

	}
}
