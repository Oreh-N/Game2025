using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSpace;

public class Movable : MonoBehaviour
{
	private void Update()
	{
		if (!MapController.Instance)
		{ Debug.Log("No map controller instance exist"); return; }
		Vector3 new_pos = MouseController.GetMouseWorldPos();
		transform.position = Map.MapToWorld(Map.WorldToMap(new_pos));
	}
}
