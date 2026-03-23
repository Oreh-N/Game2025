using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	private void Update()
	{
		if (!MapController.Instance)
		{ Debug.Log("No map controller instance exist"); return; }
		Vector3 new_pos = MapController.GetMouseWorldPos();
		transform.position = MapController.Instance.MapCoordToGrid(new_pos);
	}
}
