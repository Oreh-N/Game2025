using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	private void OnMouseDrag()
	{
		Vector3 new_pos = BuildingManager.GetMouseWorldPos();
		transform.position = BuildingManager.Instance.MapCoordToGrid(new_pos);
	}
}
